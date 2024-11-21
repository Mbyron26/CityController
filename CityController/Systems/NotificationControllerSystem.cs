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


    protected override void OnGameLoaded(Context serializationContext) {
        base.OnGameLoaded(serializationContext);
        SetElectricityNotifications();
        SetWaterPipeNotifications();
        SetBuildingNotifications();
        SetTrafficNotifications();

#if DEBUG
        Debug();
#endif

    }

    protected override void OnCreate() {
        base.OnCreate();
        logBuilder = new();
        prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
        iconQuery = GetEntityQuery(new ComponentType[]{
            ComponentType.ReadOnly<Icon>(),
            ComponentType.Exclude<Deleted>()
        });

        notificationIconDisplayDataQuery = GetEntityQuery(new ComponentType[]{
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

        RequireForUpdate(electricityParameterQuery);
        RequireForUpdate(waterPipeParameterQuery);
        RequireForUpdate(buildingConfigurationDataQuery);
        RequireForUpdate(trafficConfigurationDataQuery);
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
        World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
        }
    }

    public void SetBuildingNotifications(bool refresh = true) {
        EnableBuildingNotification(BuildingNotificationIcon.AbandonedCollapsedNotification, Setting.Instance.Notification.BuildingAbandonedCollapsedNotification);
        EnableBuildingNotification(BuildingNotificationIcon.AbandonedNotification, Setting.Instance.Notification.BuildingAbandonedNotification);
        EnableBuildingNotification(BuildingNotificationIcon.CondemnedNotification, Setting.Instance.Notification.BuildingCondemnedNotification);
        EnableBuildingNotification(BuildingNotificationIcon.TurnedOffNotification, Setting.Instance.Notification.BuildingTurnedOffNotification);
        EnableBuildingNotification(BuildingNotificationIcon.HighRentNotification, Setting.Instance.Notification.BuildingHighRentNotification);
        if (refresh)
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
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
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HighVoltageNotConnectedPrefab, value);
        }
        if (refresh) {
            World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();
        }
    }


#if DEBUG
    public void Debug() => new List<Func<string>> {
        LogElectricityNotificationSvgSources,
        LogElectricityNotificationPrefabName,
        LogWaterPipeNotificationSvgSources,
        LogWaterPipeNotificationPrefabName,
        LogBuildingNotificationSvgSources,
        LogBuildingNotificationPrefabName,
        LogTrafficNotificationSvgSources,
        LogTrafficNotificationPrefabName,
    }.ForEach(action => Logger.Info(action()));

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
