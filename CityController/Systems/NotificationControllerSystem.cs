using CityController.Data;
using CityController.Settings;
using Colossal.Serialization.Entities;
using CS2Shared.Common;
using CS2Shared.Extension;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Entities;

namespace CityController.Systems;

public partial class NotificationControllerSystem : GameSystemBaseExtension {
    private StringBuilder logBuilder;
    private EntityQuery iconQuery;
    private EntityQuery waterPipeParameterQuery;
    private PrefabSystem prefabSystem;
    private EntityQuery notificationIconDisplayDataQuery;
    private EntityQuery electricityParameterQuery;
    private EntityQuery buildingConfigurationDataQuery;
    private EntityQuery trafficConfigurationDataQuery;
    private EntityQuery companyNotificationParameterQuery;
    private EntityQuery workProviderNotificationParameterQuery;
    private EntityQuery disasterNotificationParameterQuery;
    private EntityQuery fireNotificationParameterQuery;
    private EntityQuery garbageNotificationParameterQuery;
    private EntityQuery healthcareNotificationParameterQuery;
    private EntityQuery policeNotificationParameterQuery;
    private EntityQuery pollutionNotificationParameterQuery;
    private EntityQuery resourceConsumerNotificationParameterQuery;
    private EntityQuery routeNotificationParameterQuery;
    private EntityQuery transportLineNotificationParameterQuery;

    protected override void OnGameLoaded(Context serializationContext) {
        base.OnGameLoaded(serializationContext);
        SetElectricityNotifications();
        SetWaterPipeNotifications();
        SetBuildingNotifications();
        SetTrafficNotifications();
        SetCompanyNotifications();
        SetWorkProviderNotifications();
        SetDisasterNotifications();
        SetFireNotifications();
        SetGarbageNotifications();
        SetHealthcareNotifications();
        SetPoliceNotifications();
        SetPollutionNotifications();
        SetResourceConsumerNotifications();
        SetRouteNotifications();
        SetTransportLineNotifications();
#if DEBUG
        Debug();
#endif
    }

    protected override void OnCreate() {
        base.OnCreate();
        logBuilder = new();
        prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
        iconQuery = GetEntityQuery(new ComponentType[] {
            ComponentType.ReadOnly<Icon>(),
            ComponentType.Exclude<Deleted>()
        });

        notificationIconDisplayDataQuery = GetEntityQuery(new ComponentType[] {
            ComponentType.ReadOnly<NotificationIconDisplayData>(),
        });

        electricityParameterQuery = GetEntityQuery(new ComponentType[] {
            ComponentType.ReadOnly<ElectricityParameterData>()
        });
        waterPipeParameterQuery = GetEntityQuery(new ComponentType[] {
            ComponentType.ReadOnly<WaterPipeParameterData>()
        });
        buildingConfigurationDataQuery = GetEntityQuery(new ComponentType[] {
            ComponentType.ReadOnly<BuildingConfigurationData>()
        });
        trafficConfigurationDataQuery = GetEntityQuery(new ComponentType[] {
            ComponentType.ReadOnly<TrafficConfigurationData>()
        });
        companyNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<CompanyNotificationParameterData>());
        workProviderNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<WorkProviderParameterData>());
        disasterNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<DisasterConfigurationData>());
        fireNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
        garbageNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
        healthcareNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
        policeNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
        pollutionNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
        resourceConsumerNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<ResourceConsumerData>());
        routeNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<RouteConfigurationData>());
        transportLineNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<TransportLineData>());
        RequireForUpdate(electricityParameterQuery);
        RequireForUpdate(waterPipeParameterQuery);
        RequireForUpdate(buildingConfigurationDataQuery);
        RequireForUpdate(trafficConfigurationDataQuery);
        RequireForUpdate(companyNotificationParameterQuery);
        RequireForUpdate(workProviderNotificationParameterQuery);
        RequireForUpdate(disasterNotificationParameterQuery);
        RequireForUpdate(fireNotificationParameterQuery);
        RequireForUpdate(garbageNotificationParameterQuery);
        RequireForUpdate(healthcareNotificationParameterQuery);
        RequireForUpdate(policeNotificationParameterQuery);
        RequireForUpdate(pollutionNotificationParameterQuery);
        RequireForUpdate(resourceConsumerNotificationParameterQuery);
        RequireForUpdate(routeNotificationParameterQuery);
        RequireForUpdate(transportLineNotificationParameterQuery);
    }

    private readonly Dictionary<Entity, int> EntityDictionary = new();


    public void Refresh() {
        EntityDictionary.Clear();
        NativeArray<ArchetypeChunk> nativeArray = iconQuery.ToArchetypeChunkArray(Allocator.TempJob);
        var prefabRefTypeHandle = GetComponentTypeHandle<PrefabRef>();
        for (int i = 0; i < nativeArray.Length; i++) {
            NativeArray<PrefabRef> nativeArray2 = nativeArray[i].GetNativeArray(ref prefabRefTypeHandle);
            for (int j = 0; j < nativeArray2.Length; j++) {
                Entity prefab = nativeArray2[j].m_Prefab;
                if (EntityDictionary.TryGetValue(prefab, out int num)) {
                    EntityDictionary[prefab] = num + 1;
                }
                else {
                    EntityDictionary.Add(prefab, 1);
                }
            }
        }

        nativeArray.Dispose();
        if (EntityDictionary.Any()) {
            foreach (var item in EntityDictionary) {
                Logger.Info($"{prefabSystem.GetPrefab<NotificationIconPrefab>(item.Key).name} | {item.Value}");
                //EnableNotification(item.Key, Act);
            }
        }
    }

    public void DebugNotificationIconPrefab() {
        Logger.Info($"Debug NotificationIconPrefab");
        var entityArray = notificationIconDisplayDataQuery.ToEntityArray(Allocator.TempJob);
        foreach (var item in entityArray) {
            Logger.Info($"{prefabSystem.GetPrefab<NotificationIconPrefab>(item).name}");
        }

        entityArray.Dispose();
        Logger.Info($"Debug NotificationIconPrefab completed");
    }

    public void EnableNotification(Entity entity, bool enabled) {
        EntityManager.SetComponentEnabled<NotificationIconDisplayData>(entity, enabled);
        RefreshIcon();
    }




    public void EnableTransportLineNotification(TransportLineNotificationIcon transportLineNotificationIcon, bool value, bool refresh = false) {
        var singleton = transportLineNotificationParameterQuery.GetSingleton<TransportLineData>();
        if (transportLineNotificationIcon == TransportLineNotificationIcon.VehicleNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_VehicleNotification, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetTransportLineNotifications(bool refresh = true) {
        EnableTransportLineNotification(TransportLineNotificationIcon.VehicleNotification, Setting.Instance.Notification.TransportLineVehicleNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableRouteNotification(RouteNotificationIcon routeNotificationIcon, bool value, bool refresh = false) {
        var singleton = routeNotificationParameterQuery.GetSingleton<RouteConfigurationData>();
        if (routeNotificationIcon == RouteNotificationIcon.PathfindNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_PathfindNotification, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetRouteNotifications(bool refresh = true) {
        EnableRouteNotification(RouteNotificationIcon.PathfindNotification, Setting.Instance.Notification.RoutePathfindNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableResourceConsumerNotification(ResourceConsumerNotificationIcon resourceConsumerNotificationIcon, bool value, bool refresh = false) {
        var singleton = resourceConsumerNotificationParameterQuery.GetSingleton<ResourceConsumerData>();
        if (resourceConsumerNotificationIcon == ResourceConsumerNotificationIcon.NoResourceNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoResourceNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetResourceConsumerNotifications(bool refresh = true) {
        EnableResourceConsumerNotification(ResourceConsumerNotificationIcon.NoResourceNotification, Setting.Instance.Notification.ResourceConsumerNoResourceNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnablePollutionNotification(PollutionNotificationIcon pollutionNotificationIcon, bool value, bool refresh = false) {
        var singleton = pollutionNotificationParameterQuery.GetSingleton<PollutionParameterData>();
        if (pollutionNotificationIcon == PollutionNotificationIcon.AirPollutionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AirPollutionNotification, value);
        }
        else if (pollutionNotificationIcon == PollutionNotificationIcon.NoisePollutionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoisePollutionNotification, value);
        }
        else if (pollutionNotificationIcon == PollutionNotificationIcon.GroundPollutionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_GroundPollutionNotification, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetPollutionNotifications(bool refresh = true) {
        EnablePollutionNotification(PollutionNotificationIcon.AirPollutionNotification, Setting.Instance.Notification.PollutionAirPollutionNotification);
        EnablePollutionNotification(PollutionNotificationIcon.NoisePollutionNotification, Setting.Instance.Notification.PollutionNoisePollutionNotification);
        EnablePollutionNotification(PollutionNotificationIcon.GroundPollutionNotification, Setting.Instance.Notification.PollutionGroundPollutionNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnablePoliceNotification(PoliceNotificationIcon policeNotificationIcon, bool value, bool refresh = false) {
        var singleton = policeNotificationParameterQuery.GetSingleton<PoliceConfigurationData>();
        if (policeNotificationIcon == PoliceNotificationIcon.TrafficAccidentNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TrafficAccidentNotificationPrefab, value);
        }
        else if (policeNotificationIcon == PoliceNotificationIcon.CrimeSceneNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_CrimeSceneNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetPoliceNotifications(bool refresh = true) {
        EnablePoliceNotification(PoliceNotificationIcon.TrafficAccidentNotification, Setting.Instance.Notification.PoliceTrafficAccidentNotification);
        EnablePoliceNotification(PoliceNotificationIcon.CrimeSceneNotification, Setting.Instance.Notification.PoliceCrimeSceneNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableHealthcareNotification(HealthcareNotificationIcon healthcareNotificationIcon, bool value, bool refresh = false) {
        var singleton = healthcareNotificationParameterQuery.GetSingleton<HealthcareParameterData>();
        if (healthcareNotificationIcon == HealthcareNotificationIcon.AmbulanceNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AmbulanceNotificationPrefab, value);
        }
        else if (healthcareNotificationIcon == HealthcareNotificationIcon.HearseNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HearseNotificationPrefab, value);
        }
        else if (healthcareNotificationIcon == HealthcareNotificationIcon.FacilityFullNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_FacilityFullNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetHealthcareNotifications(bool refresh = true) {
        EnableHealthcareNotification(HealthcareNotificationIcon.AmbulanceNotification, Setting.Instance.Notification.HealthcareAmbulanceNotification);
        EnableHealthcareNotification(HealthcareNotificationIcon.HearseNotification, Setting.Instance.Notification.HealthcareHearseNotification);
        EnableHealthcareNotification(HealthcareNotificationIcon.FacilityFullNotification, Setting.Instance.Notification.HealthcareFacilityFullNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableGarbageNotification(GarbageNotificationIcon garbageNotificationIcon, bool value, bool refresh = false) {
        var singleton = garbageNotificationParameterQuery.GetSingleton<GarbageParameterData>();
        if (garbageNotificationIcon == GarbageNotificationIcon.GarbageNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_GarbageNotificationPrefab, value);
        }
        else if (garbageNotificationIcon == GarbageNotificationIcon.FacilityFullNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_FacilityFullNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetGarbageNotifications(bool refresh = true) {
        EnableGarbageNotification(GarbageNotificationIcon.GarbageNotification, Setting.Instance.Notification.GarbageGarbageNotification);
        EnableGarbageNotification(GarbageNotificationIcon.FacilityFullNotification, Setting.Instance.Notification.GarbageFacilityFullNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableFireNotification(FireNotificationIcon fireNotificationIcon, bool value, bool refresh = false) {
        var singleton = fireNotificationParameterQuery.GetSingleton<FireConfigurationData>();
        if (fireNotificationIcon == FireNotificationIcon.FireNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_FireNotificationPrefab, value);
        }
        else if (fireNotificationIcon == FireNotificationIcon.BurnedDownNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BurnedDownNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetFireNotifications(bool refresh = true) {
        EnableFireNotification(FireNotificationIcon.FireNotification, Setting.Instance.Notification.FireFireNotification);
        EnableFireNotification(FireNotificationIcon.BurnedDownNotification, Setting.Instance.Notification.FireBurnedDownNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableDisasterNotification(DisasterNotificationIcon disasterNotificationIcon, bool value, bool refresh = false) {
        var singleton = disasterNotificationParameterQuery.GetSingleton<DisasterConfigurationData>();
        if (disasterNotificationIcon == DisasterNotificationIcon.WeatherDamageNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WeatherDamageNotificationPrefab, value);
        }
        else if (disasterNotificationIcon == DisasterNotificationIcon.WeatherDestroyedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WeatherDestroyedNotificationPrefab, value);
        }
        else if (disasterNotificationIcon == DisasterNotificationIcon.WaterDamageNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterDamageNotificationPrefab, value);
        }
        else if (disasterNotificationIcon == DisasterNotificationIcon.WaterDestroyedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterDestroyedNotificationPrefab, value);
        }
        else if (disasterNotificationIcon == DisasterNotificationIcon.DestroyedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DestroyedNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetDisasterNotifications(bool refresh = true) {
        EnableDisasterNotification(DisasterNotificationIcon.WeatherDamageNotification, Setting.Instance.Notification.DisasterWeatherDamageNotification);
        EnableDisasterNotification(DisasterNotificationIcon.WeatherDestroyedNotification, Setting.Instance.Notification.DisasterWeatherDestroyedNotification);
        EnableDisasterNotification(DisasterNotificationIcon.WaterDamageNotification, Setting.Instance.Notification.DisasterWaterDamageNotification);
        EnableDisasterNotification(DisasterNotificationIcon.WaterDestroyedNotification, Setting.Instance.Notification.DisasterWaterDestroyedNotification);
        EnableDisasterNotification(DisasterNotificationIcon.DestroyedNotification, Setting.Instance.Notification.DisasterDestroyedNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableWorkProviderNotification(WorkProviderNotificationIcon workProviderNotificationIcon, bool value, bool refresh = false) {
        var singleton = workProviderNotificationParameterQuery.GetSingleton<WorkProviderParameterData>();
        if (workProviderNotificationIcon == WorkProviderNotificationIcon.UneducatedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_UneducatedNotificationPrefab, value);
        }
        else if (workProviderNotificationIcon == WorkProviderNotificationIcon.EducatedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_EducatedNotificationPrefab, value);
        }
        if (refresh)
            RefreshIcon();
    }

    public void SetWorkProviderNotifications(bool refresh = true) {
        EnableWorkProviderNotification(WorkProviderNotificationIcon.UneducatedNotification, Setting.Instance.Notification.WorkProviderUneducatedNotification);
        EnableWorkProviderNotification(WorkProviderNotificationIcon.EducatedNotification, Setting.Instance.Notification.WorkProviderEducatedNotification);
        if (refresh)
            RefreshIcon();
    }

    public void SetCompanyNotifications(bool refresh = true) {
        EnableCompanyNotification(CompanyNotificationIcon.NoInputsNotification, Setting.Instance.Notification.CompanyNoInputsNotification);
        EnableCompanyNotification(CompanyNotificationIcon.NoCustomersNotification, Setting.Instance.Notification.CompanyNoCustomersNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableCompanyNotification(CompanyNotificationIcon companyNotificationIcon, bool value, bool refresh = false) {
        var singleton = companyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>();
        if (companyNotificationIcon == CompanyNotificationIcon.NoInputsNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoInputsNotificationPrefab, value);
        }
        else if (companyNotificationIcon == CompanyNotificationIcon.NoCustomersNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoCustomersNotificationPrefab, value);
        }
        if (refresh) {
            RefreshIcon();
        }
    }

    public void SetTrafficNotifications(bool refresh = true) {
        EnableTrafficNotification(TrafficNotificationIcon.BottleneckNotification, Setting.Instance.Notification.TrafficBottleneckNotification);
        EnableTrafficNotification(TrafficNotificationIcon.DeadEndNotification, Setting.Instance.Notification.TrafficDeadEndNotification);
        EnableTrafficNotification(TrafficNotificationIcon.RoadConnectionNotification, Setting.Instance.Notification.TrafficRoadConnectionNotification);
        EnableTrafficNotification(TrafficNotificationIcon.TrackConnectionNotification, Setting.Instance.Notification.TrafficTrackConnectionNotification);
        EnableTrafficNotification(TrafficNotificationIcon.CarConnectionNotification, Setting.Instance.Notification.TrafficCarConnectionNotification);
        EnableTrafficNotification(TrafficNotificationIcon.ShipConnectionNotification, Setting.Instance.Notification.TrafficShipConnectionNotification);
        EnableTrafficNotification(TrafficNotificationIcon.TrainConnectionNotification, Setting.Instance.Notification.TrafficTrainConnectionNotification);
        EnableTrafficNotification(TrafficNotificationIcon.PedestrianConnectionNotification, Setting.Instance.Notification.TrafficPedestrianConnectionNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableTrafficNotification(TrafficNotificationIcon trafficNotificationIcon, bool value, bool refresh = false) {
        var singleton = trafficConfigurationDataQuery.GetSingleton<TrafficConfigurationData>();
        if (trafficNotificationIcon == TrafficNotificationIcon.BottleneckNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BottleneckNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.DeadEndNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DeadEndNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.RoadConnectionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_RoadConnectionNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.TrackConnectionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TrackConnectionNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.CarConnectionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_CarConnectionNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.ShipConnectionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_ShipConnectionNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.TrainConnectionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TrainConnectionNotification, value);
        }
        else if (trafficNotificationIcon == TrafficNotificationIcon.PedestrianConnectionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_PedestrianConnectionNotification, value);
        }

        if (refresh) {
            RefreshIcon();
        }
    }

    public void SetBuildingNotifications(bool refresh = true) {
        EnableBuildingNotification(BuildingNotificationIcon.AbandonedCollapsedNotification, Setting.Instance.Notification.BuildingAbandonedCollapsedNotification);
        EnableBuildingNotification(BuildingNotificationIcon.AbandonedNotification, Setting.Instance.Notification.BuildingAbandonedNotification);
        EnableBuildingNotification(BuildingNotificationIcon.CondemnedNotification, Setting.Instance.Notification.BuildingCondemnedNotification);
        EnableBuildingNotification(BuildingNotificationIcon.TurnedOffNotification, Setting.Instance.Notification.BuildingTurnedOffNotification);
        EnableBuildingNotification(BuildingNotificationIcon.HighRentNotification, Setting.Instance.Notification.BuildingHighRentNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableBuildingNotification(BuildingNotificationIcon buildingNotificationIcon, bool value, bool refresh = false) {
        var singleton = buildingConfigurationDataQuery.GetSingleton<BuildingConfigurationData>();
        if (buildingNotificationIcon == BuildingNotificationIcon.AbandonedCollapsedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AbandonedCollapsedNotification, value);
        }
        else if (buildingNotificationIcon == BuildingNotificationIcon.AbandonedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AbandonedNotification, value);
        }
        else if (buildingNotificationIcon == BuildingNotificationIcon.CondemnedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_CondemnedNotification, value);
        }
        else if (buildingNotificationIcon == BuildingNotificationIcon.TurnedOffNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TurnedOffNotification, value);
        }
        else if (buildingNotificationIcon == BuildingNotificationIcon.HighRentNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HighRentNotification, value);
        }

        if (refresh)
            RefreshIcon();
    }

    public void SetWaterPipeNotifications(bool refresh = true) {
        EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterNotification, Setting.Instance.Notification.WaterPipeWaterNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterNotification, Setting.Instance.Notification.WaterPipeDirtyWaterNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.SewageNotification, Setting.Instance.Notification.WaterPipeSewageNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterPipeNotConnectedNotification, Setting.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.SewagePipeNotConnectedNotification, Setting.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughWaterCapacityNotification, Setting.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSewageCapacityNotification, Setting.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughGroundwaterNotification, Setting.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSurfaceWaterNotification, Setting.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification);
        EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterPumpNotification, Setting.Instance.Notification.WaterPipeDirtyWaterPumpNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableWaterPipeNotification(WaterPipeNotificationIcon waterPipeNotificationIcon, bool value, bool refresh = false) {
        var singleton = waterPipeParameterQuery.GetSingleton<WaterPipeParameterData>();
        if (waterPipeNotificationIcon == WaterPipeNotificationIcon.WaterNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.DirtyWaterNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DirtyWaterNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.SewageNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_SewageNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.WaterPipeNotConnectedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterPipeNotConnectedNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.SewagePipeNotConnectedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_SewagePipeNotConnectedNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughWaterCapacityNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughWaterCapacityNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughSewageCapacityNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughSewageCapacityNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughGroundwaterNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughGroundwaterNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughSurfaceWaterNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughSurfaceWaterNotification, value);
        }
        else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.DirtyWaterPumpNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DirtyWaterPumpNotification, value);
        }

        if (refresh) {
            RefreshIcon();
        }
    }

    public void SetElectricityNotifications(bool refresh = true) {
        EnableElectricityNotification(ElectricityNotificationIcon.ElectricityNotification, Setting.Instance.Notification.ElectricityElectricityNotification);
        EnableElectricityNotification(ElectricityNotificationIcon.BottleneckNotification, Setting.Instance.Notification.ElectricityBottleneckNotification);
        EnableElectricityNotification(ElectricityNotificationIcon.BuildingBottleneckNotification, Setting.Instance.Notification.ElectricityBuildingBottleneckNotification);
        EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughProductionNotification, Setting.Instance.Notification.ElectricityNotEnoughProductionNotification);
        EnableElectricityNotification(ElectricityNotificationIcon.TransformerNotification, Setting.Instance.Notification.ElectricityTransformerNotification);
        EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughConnectedNotification, Setting.Instance.Notification.ElectricityNotEnoughConnectedNotification);
        EnableElectricityNotification(ElectricityNotificationIcon.BatteryEmptyNotification, Setting.Instance.Notification.ElectricityBatteryEmptyNotification);
        if (refresh)
            RefreshIcon();
    }

    public void EnableElectricityNotification(ElectricityNotificationIcon electricityNotificationIcon, bool value, bool refresh = false) {
        var singleton = electricityParameterQuery.GetSingleton<ElectricityParameterData>();
        if (electricityNotificationIcon == ElectricityNotificationIcon.ElectricityNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_ElectricityNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.BottleneckNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BottleneckNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.BuildingBottleneckNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BuildingBottleneckNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.NotEnoughProductionNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughProductionNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.TransformerNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TransformerNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.NotEnoughConnectedNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughConnectedNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.BatteryEmptyNotification) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BatteryEmptyNotificationPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.LowVoltageNotConnected) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_LowVoltageNotConnectedPrefab, value);
        }
        else if (electricityNotificationIcon == ElectricityNotificationIcon.HighVoltageNotConnected) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HighVoltageNotConnectedPrefab,
                value);
        }

        if (refresh) {
            RefreshIcon();
        }
    }

    public void RefreshIcon() => World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();

#if DEBUG
    public void Debug() => new List<Func<string>> {
        //LogElectricityNotificationSvgSources,
        //LogElectricityNotificationPrefabName,
        //LogWaterPipeNotificationSvgSources,
        //LogWaterPipeNotificationPrefabName,
        //LogBuildingNotificationSvgSources,
        //LogBuildingNotificationPrefabName,
        //LogTrafficNotificationSvgSources,
        //LogTrafficNotificationPrefabName,
        //LogCompanyNotificationSvgSources,
        //LogCompanyNotificationPrefabName,
        //LogWorkProviderNotificationSvgSources,
        //LogWorkProviderNotificationPrefabName
        //LogDisasterNotificationSvgSources,
        //LogDisasterNotificationPrefabName,
        //LogFireNotificationSvgSources,
        //LogFireNotificationPrefabName,
        //LogGarbageNotificationSvgSources,
        //LogGarbageNotificationPrefabName,
        //LogHealthcareNotificationSvgSources,
        //LogHealthcareNotificationPrefabName,
        //LogPoliceNotificationSvgSources,
        //LogPoliceNotificationPrefabName,
        //LogPollutionNotificationSvgSources,
        //LogPollutionNotificationPrefabName,
        //LogResourceConsumerNotificationSvgSources,
        //LogResourceConsumerNotificationPrefabName,
        //LogRouteNotificationSvgSources,
        //LogRouteNotificationPrefabName,
        //LogTransportLineNotificationSvgSources,
        //LogTransportLineNotificationPrefabName,
    }.ForEach(action => Logger.Info(action()));

    private List<NotificationIconPrefab> GetTransportLineNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = transportLineNotificationParameterQuery.GetSingleton<TransportLineData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_VehicleNotification));
        return result;
    }

    private List<string> GetTransportLineNotificationSvg() => GetTransportLineNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogTransportLineNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogTransportLineNotificationSvgSources")).ToString(_ => GetTransportLineNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetTransportLineNotificationPrefabName() {
        List<string> result = new();
        var singleton = transportLineNotificationParameterQuery.GetSingleton<TransportLineData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_VehicleNotification).name);
        return result;
    }

    private string LogTransportLineNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogTransportLineNotificationPrefabName")).ToString(_ => GetTransportLineNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetRouteNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = routeNotificationParameterQuery.GetSingleton<RouteConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PathfindNotification));
        return result;
    }

    private List<string> GetRouteNotificationSvg() => GetRouteNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogRouteNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogRouteNotificationSvgSources")).ToString(_ => GetRouteNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetRouteNotificationPrefabName() {
        List<string> result = new();
        var singleton = routeNotificationParameterQuery.GetSingleton<Game.Prefabs.RouteConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PathfindNotification).name);
        return result;
    }

    private string LogRouteNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogRouteNotificationPrefabName")).ToString(_ => GetRouteNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetResourceConsumerNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = resourceConsumerNotificationParameterQuery.GetSingleton<ResourceConsumerData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoResourceNotificationPrefab));
        return result;
    }

    private List<string> GetResourceConsumerNotificationSvg() => GetResourceConsumerNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogResourceConsumerNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogResourceConsumerNotificationSvgSources")).ToString(_ => GetResourceConsumerNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetResourceConsumerNotificationPrefabName() {
        List<string> result = new();
        var singleton = resourceConsumerNotificationParameterQuery.GetSingleton<Game.Prefabs.ResourceConsumerData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoResourceNotificationPrefab).name);
        return result;
    }

    private string LogResourceConsumerNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogResourceConsumerNotificationPrefabName")).ToString(_ => GetResourceConsumerNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetPollutionNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = pollutionNotificationParameterQuery.GetSingleton<PollutionParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AirPollutionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoisePollutionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GroundPollutionNotification));
        return result;
    }

    private List<string> GetPollutionNotificationSvg() => GetPollutionNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogPollutionNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogPollutionNotificationSvgSources")).ToString(_ => GetPollutionNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetPollutionNotificationPrefabName() {
        List<string> result = new();
        var singleton = pollutionNotificationParameterQuery.GetSingleton<Game.Prefabs.PollutionParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AirPollutionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoisePollutionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GroundPollutionNotification).name);
        return result;
    }

    private string LogPollutionNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogPollutionNotificationPrefabName")).ToString(_ => GetPollutionNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetPoliceNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = policeNotificationParameterQuery.GetSingleton<PoliceConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrafficAccidentNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CrimeSceneNotificationPrefab));
        return result;
    }

    private List<string> GetPoliceNotificationSvg() => GetPoliceNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogPoliceNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogPoliceNotificationSvgSources")).ToString(_ => GetPoliceNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetPoliceNotificationPrefabName() {
        List<string> result = new();
        var singleton = policeNotificationParameterQuery.GetSingleton<Game.Prefabs.PoliceConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrafficAccidentNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CrimeSceneNotificationPrefab).name);
        return result;
    }

    private string LogPoliceNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogPoliceNotificationPrefabName")).ToString(_ => GetPoliceNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetHealthcareNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = healthcareNotificationParameterQuery.GetSingleton<HealthcareParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AmbulanceNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HearseNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab));
        return result;
    }

    private List<string> GetHealthcareNotificationSvg() => GetHealthcareNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogHealthcareNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogHealthcareNotificationSvgSources")).ToString(_ => GetHealthcareNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetHealthcareNotificationPrefabName() {
        List<string> result = new();
        var singleton = healthcareNotificationParameterQuery.GetSingleton<Game.Prefabs.HealthcareParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AmbulanceNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HearseNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab).name);
        return result;
    }

    private string LogHealthcareNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogHealthcareNotificationPrefabName")).ToString(_ => GetHealthcareNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetGarbageNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = garbageNotificationParameterQuery.GetSingleton<GarbageParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GarbageNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab));
        return result;
    }

    private List<string> GetGarbageNotificationSvg() => GetGarbageNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogGarbageNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogGarbageNotificationSvgSources")).ToString(_ => GetGarbageNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetGarbageNotificationPrefabName() {
        List<string> result = new();
        var singleton = garbageNotificationParameterQuery.GetSingleton<Game.Prefabs.GarbageParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GarbageNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab).name);
        return result;
    }

    private string LogGarbageNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogGarbageNotificationPrefabName")).ToString(_ => GetGarbageNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<NotificationIconPrefab> GetFireNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = fireNotificationParameterQuery.GetSingleton<FireConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FireNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BurnedDownNotificationPrefab));
        return result;
    }

    private List<string> GetFireNotificationSvg() => GetFireNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogFireNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogFireNotificationSvgSources")).ToString(_ => GetFireNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetFireNotificationPrefabName() {
        List<string> result = new();
        var singleton = fireNotificationParameterQuery.GetSingleton<Game.Prefabs.FireConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FireNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BurnedDownNotificationPrefab).name);
        return result;
    }

    private string LogFireNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogFireNotificationPrefabName")).ToString(_ => GetFireNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);


    private List<NotificationIconPrefab> GetDisasterNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = disasterNotificationParameterQuery.GetSingleton<DisasterConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDamageNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDestroyedNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDamageNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDestroyedNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DestroyedNotificationPrefab));
        return result;
    }

    private List<string> GetDisasterNotificationSvg() => GetDisasterNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private string LogDisasterNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogDisasterNotificationSvgSources")).ToString(_ => GetDisasterNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetDisasterNotificationPrefabName() {
        List<string> result = new();
        var singleton = disasterNotificationParameterQuery.GetSingleton<Game.Prefabs.DisasterConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDamageNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDestroyedNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDamageNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDestroyedNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DestroyedNotificationPrefab).name);
        return result;
    }

    private string LogDisasterNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogDisasterNotificationPrefabName")).ToString(_ => GetDisasterNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private string LogWorkProviderNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogWorkProviderNotificationPrefabName")).ToString(_ => GetWorkProviderNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetWorkProviderNotificationPrefabName() {
        List<string> result = new();
        var singleton = workProviderNotificationParameterQuery.GetSingleton<Game.Prefabs.WorkProviderParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_UneducatedNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_EducatedNotificationPrefab).name);
        return result;
    }

    private string LogWorkProviderNotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag("LogWorkProviderNotificationSvgSources")).ToString(_ => GetWorkProviderNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

    private List<string> GetWorkProviderNotificationSvg() => GetWorkProviderNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private List<NotificationIconPrefab> GetWorkProviderNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = workProviderNotificationParameterQuery.GetSingleton<WorkProviderParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_UneducatedNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_EducatedNotificationPrefab));
        return result;
    }

    private string LogCompanyNotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag("LogCompanyNotificationPrefabName")).ToString(_ => GetCompanyNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);


    private List<string> GetCompanyNotificationPrefabName() {
        List<string> result = new();
        var singleton = companyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoInputsNotificationPrefab).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoCustomersNotificationPrefab).name);
        return result;
    }

    private string LogCompanyNotificationSvgSources() {
        logBuilder.ClearAndAppendLine(LogFlag("LogCompanyNotificationSvgSources"));
        return logBuilder.ToString(_ => GetCompanyNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);
    }

    private List<string> GetCompanyNotificationSvg() => GetCompanyNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private List<NotificationIconPrefab> GetCompanyNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = companyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoInputsNotificationPrefab));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoCustomersNotificationPrefab));
        return result;
    }

    private string LogTrafficNotificationPrefabName() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogTrafficNotificationPrefabName"));
        logBuilder.ToString(_ => GetTrafficNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetTrafficNotificationPrefabName() {
        List<string> result = new();
        var singleton = trafficConfigurationDataQuery.GetSingleton<TrafficConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DeadEndNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_RoadConnectionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrackConnectionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CarConnectionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ShipConnectionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrainConnectionNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PedestrianConnectionNotification).name);
        return result;
    }

    private string LogTrafficNotificationSvgSources() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogTrafficNotificationSvgSources"));
        logBuilder.ToString(_ => GetTrafficNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetTrafficNotificationSvg() => GetTrafficNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private List<NotificationIconPrefab> GetTrafficNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = trafficConfigurationDataQuery.GetSingleton<TrafficConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DeadEndNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_RoadConnectionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrackConnectionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CarConnectionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ShipConnectionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrainConnectionNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PedestrianConnectionNotification));
        return result;
    }

    private string LogBuildingNotificationPrefabName() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogBuildingNotificationPrefabName"));
        logBuilder.ToString(_ => GetBuildingNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetBuildingNotificationPrefabName() {
        List<string> result = new();
        var singleton = buildingConfigurationDataQuery.GetSingleton<BuildingConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedCollapsedNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CondemnedNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LevelUpNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TurnedOffNotification).name);
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighRentNotification).name);
        return result;
    }

    private string LogBuildingNotificationSvgSources() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogBuildingNotificationSvgSources"));
        logBuilder.ToString(_ => GetBuildingNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetBuildingNotificationSvg() => GetBuildingNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private List<NotificationIconPrefab> GetBuildingNotificationPrefab() {
        List<NotificationIconPrefab> result = new();
        var singleton = buildingConfigurationDataQuery.GetSingleton<BuildingConfigurationData>();
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedCollapsedNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CondemnedNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LevelUpNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TurnedOffNotification));
        result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighRentNotification));
        return result;
    }

    private string LogWaterPipeNotificationPrefabName() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogWaterPipeNotificationPrefabName"));
        logBuilder.ToString(_ => GetWaterPipeNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetWaterPipeNotificationPrefabName() {
        List<string> name = new();
        var singleton = waterPipeParameterQuery.GetSingleton<WaterPipeParameterData>();
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewageNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterPipeNotConnectedNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewagePipeNotConnectedNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughWaterCapacityNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSewageCapacityNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughGroundwaterNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSurfaceWaterNotification).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterPumpNotification).name);
        return name;
    }

    private string LogWaterPipeNotificationSvgSources() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogWaterPipeNotificationSvgSources"));
        logBuilder.ToString(_ => GetWaterPipeNotificationSvgSources().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetWaterPipeNotificationSvgSources() => GetWaterPipeNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private List<NotificationIconPrefab> GetWaterPipeNotificationPrefab() {
        List<NotificationIconPrefab> notificationIconPrefabs = new();
        var singleton = waterPipeParameterQuery.GetSingleton<WaterPipeParameterData>();
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewageNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterPipeNotConnectedNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewagePipeNotConnectedNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughWaterCapacityNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSewageCapacityNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughGroundwaterNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSurfaceWaterNotification));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterPumpNotification));
        return notificationIconPrefabs;
    }

    private string LogElectricityNotificationPrefabName() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogElectricityNotificationPrefabName"));
        logBuilder.ToString(_ => GetElectricityNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetElectricityNotificationPrefabName() {
        List<string> name = new();
        var singleton = electricityParameterQuery.GetSingleton<ElectricityParameterData>();
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ElectricityNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BuildingBottleneckNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughProductionNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TransformerNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughConnectedNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BatteryEmptyNotificationPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LowVoltageNotConnectedPrefab).name);
        name.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighVoltageNotConnectedPrefab).name);
        return name;
    }

    private string LogElectricityNotificationSvgSources() {
        logBuilder.Clear();
        logBuilder.AppendLine(LogFlag("LogElectricityNotificationSvgSources"));
        logBuilder.ToString(_ => GetElectricityNotificationSvgSources().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        return logBuilder.ToString();
    }

    private List<string> GetElectricityNotificationSvgSources() => GetElectricityNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

    private List<NotificationIconPrefab> GetElectricityNotificationPrefab() {
        List<NotificationIconPrefab> notificationIconPrefabs = new();
        var singleton = electricityParameterQuery.GetSingleton<ElectricityParameterData>();
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ElectricityNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BuildingBottleneckNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughProductionNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TransformerNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughConnectedNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BatteryEmptyNotificationPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LowVoltageNotConnectedPrefab));
        notificationIconPrefabs.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighVoltageNotConnectedPrefab));
        return notificationIconPrefabs;
    }

    private string LogFlag(string name) => $"--- {name} ---";

#endif
}