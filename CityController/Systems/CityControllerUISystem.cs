using CityController.Data;
using CityController.Settings;
using CS2Shared.Common;

namespace CityController.Systems;

public partial class CityControllerUISystem : UISystemBaseExtension {
    private NotificationControllerSystem notificationControllerSystem;

    private BoolBinding panelVisibleBinding;

    private BoolBinding electricityElectricityNotificationBinding;
    private BoolBinding electricityBottleneckNotificationBinding;
    private BoolBinding electricityBuildingBottleneckNotificationBinding;
    private BoolBinding electricityNotEnoughProductionNotificationBinding;
    private BoolBinding electricityTransformerNotificationBinding;
    private BoolBinding electricityNotEnoughConnectedNotificationBinding;
    private BoolBinding electricityBatteryEmptyNotificationBinding;
    private BoolBinding electricityLowVoltageNotConnectedBinding;
    private BoolBinding electricityHighVoltageNotConnectedBinding;

    private BoolBinding waterPipeWaterNotificationBinding;
    private BoolBinding waterPipeDirtyWaterNotificationBinding;
    private BoolBinding waterPipeSewageNotificationBinding;
    private BoolBinding waterPipeWaterPipeNotConnectedNotificationBinding;
    private BoolBinding waterPipeSewagePipeNotConnectedNotificationBinding;
    private BoolBinding waterPipeNotEnoughWaterCapacityNotificationBinding;
    private BoolBinding waterPipeNotEnoughSewageCapacityNotificationBinding;
    private BoolBinding waterPipeNotEnoughGroundwaterNotificationBinding;
    private BoolBinding waterPipeNotEnoughSurfaceWaterNotificationBinding;
    private BoolBinding waterPipeDirtyWaterPumpNotificationBinding;

    private BoolBinding buildingAbandonedCollapsedNotificationBinding;
    private BoolBinding buildingAbandonedNotificationBinding;
    private BoolBinding buildingCondemnedNotificationBinding;
    private BoolBinding buildingTurnedOffNotificationBinding;
    private BoolBinding buildingHighRentNotificationBinding;

    private BoolBinding trafficBottleneckNotificationBinding;
    private BoolBinding trafficDeadEndNotificationBinding;
    private BoolBinding trafficRoadConnectionNotificationBinding;
    private BoolBinding trafficTrackConnectionNotificationBinding;
    private BoolBinding trafficCarConnectionNotificationBinding;
    private BoolBinding trafficShipConnectionNotificationBinding;
    private BoolBinding trafficTrainConnectionNotificationBinding;
    private BoolBinding trafficPedestrianConnectionNotificationBinding;

    protected override void OnCreate() {
        base.OnCreate();
        notificationControllerSystem = World.GetOrCreateSystemManaged<NotificationControllerSystem>();

        panelVisibleBinding = AddBoolBindingAndTriggerBinding("ControlPanelEnabled", false, OnControlPanelBindingToggle);

        electricityElectricityNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityElectricityNotification), Setting.Instance.Notification.ElectricityElectricityNotification, OnElectricityElectricityNotificationToggle);
        electricityBottleneckNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityBottleneckNotification), Setting.Instance.Notification.ElectricityBottleneckNotification, OnElectricityBottleneckNotificationToggle);
        electricityBuildingBottleneckNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityBuildingBottleneckNotification), Setting.Instance.Notification.ElectricityBuildingBottleneckNotification, OnElectricityBuildingBottleneckNotificationToggle);
        electricityNotEnoughProductionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityNotEnoughProductionNotification), Setting.Instance.Notification.ElectricityNotEnoughProductionNotification, OnElectricityNotEnoughProductionNotificationToggle);
        electricityTransformerNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityTransformerNotification), Setting.Instance.Notification.ElectricityTransformerNotification, OnElectricityTransformerNotificationToggle);
        electricityNotEnoughConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityNotEnoughConnectedNotification), Setting.Instance.Notification.ElectricityNotEnoughConnectedNotification, OnElectricityNotEnoughConnectedNotificationToggle);
        electricityBatteryEmptyNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityBatteryEmptyNotification), Setting.Instance.Notification.ElectricityBatteryEmptyNotification, OnElectricityBatteryEmptyNotificationToggle);
        electricityLowVoltageNotConnectedBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityLowVoltageNotConnected), Setting.Instance.Notification.ElectricityLowVoltageNotConnected, OnElectricityLowVoltageNotConnectedToggle);
        electricityHighVoltageNotConnectedBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.ElectricityHighVoltageNotConnected), Setting.Instance.Notification.ElectricityHighVoltageNotConnected, OnElectricityHighVoltageNotConnectedToggle);

        waterPipeWaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeWaterNotification), Setting.Instance.Notification.WaterPipeWaterNotification, OnWaterPipeWaterNotificationToggle);
        waterPipeDirtyWaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeDirtyWaterNotification), Setting.Instance.Notification.WaterPipeDirtyWaterNotification, OnWaterPipeDirtyWaterNotificationToggle);
        waterPipeSewageNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeSewageNotification), Setting.Instance.Notification.WaterPipeSewageNotification, OnWaterPipeSewageNotificationToggle);
        waterPipeWaterPipeNotConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification), Setting.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification, OnWaterPipeWaterPipeNotConnectedNotificationToggle);
        waterPipeSewagePipeNotConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification), Setting.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification, OnWaterPipeSewagePipeNotConnectedNotificationToggle);
        waterPipeNotEnoughWaterCapacityNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification), Setting.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification, OnWaterPipeNotEnoughWaterCapacityNotificationToggle);
        waterPipeNotEnoughSewageCapacityNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification), Setting.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification, OnWaterPipeNotEnoughSewageCapacityNotificationToggle);
        waterPipeNotEnoughGroundwaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification), Setting.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification, OnWaterPipeNotEnoughGroundwaterNotificationToggle);
        waterPipeNotEnoughSurfaceWaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification), Setting.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification, OnWaterPipeNotEnoughSurfaceWaterNotificationToggle);
        waterPipeDirtyWaterPumpNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.WaterPipeDirtyWaterPumpNotification), Setting.Instance.Notification.WaterPipeDirtyWaterPumpNotification, OnWaterPipeDirtyWaterPumpNotificationToggle);

        buildingAbandonedCollapsedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.BuildingAbandonedCollapsedNotification), Setting.Instance.Notification.BuildingAbandonedCollapsedNotification, OnBuildingAbandonedCollapsedNotificationToggle);
        buildingAbandonedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.BuildingAbandonedNotification), Setting.Instance.Notification.BuildingAbandonedNotification, OnBuildingAbandonedNotificationToggle);
        buildingCondemnedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.BuildingCondemnedNotification), Setting.Instance.Notification.BuildingCondemnedNotification, OnBuildingCondemnedNotificationToggle);
        buildingTurnedOffNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.BuildingTurnedOffNotification), Setting.Instance.Notification.BuildingTurnedOffNotification, OnBuildingTurnedOffNotificationToggle);
        buildingHighRentNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.BuildingHighRentNotification), Setting.Instance.Notification.BuildingHighRentNotification, OnBuildingHighRentNotificationToggle);

        trafficBottleneckNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficBottleneckNotification), Setting.Instance.Notification.TrafficBottleneckNotification, OnTrafficBottleneckNotificationToggle);
        trafficDeadEndNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficDeadEndNotification), Setting.Instance.Notification.TrafficDeadEndNotification, OnTrafficDeadEndNotificationToggle);
        trafficRoadConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficRoadConnectionNotification), Setting.Instance.Notification.TrafficRoadConnectionNotification, OnTrafficRoadConnectionNotificationToggle);
        trafficTrackConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficTrackConnectionNotification), Setting.Instance.Notification.TrafficTrackConnectionNotification, OnTrafficTrackConnectionNotificationToggle);
        trafficCarConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficCarConnectionNotification), Setting.Instance.Notification.TrafficCarConnectionNotification, OnTrafficCarConnectionNotificationToggle);
        trafficShipConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficShipConnectionNotification), Setting.Instance.Notification.TrafficShipConnectionNotification, OnTrafficShipConnectionNotificationToggle);
        trafficTrainConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficTrainConnectionNotification), Setting.Instance.Notification.TrafficTrainConnectionNotification, OnTrafficTrainConnectionNotificationToggle);
        trafficPedestrianConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.TrafficPedestrianConnectionNotification), Setting.Instance.Notification.TrafficPedestrianConnectionNotification, OnTrafficPedestrianConnectionNotificationToggle);
    }



    #region OnElectricityNotificationToggle
    private void OnElectricityElectricityNotificationToggle(bool value) {
        electricityElectricityNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityElectricityNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.ElectricityNotification, value, true);
    }
    private void OnElectricityBottleneckNotificationToggle(bool value) {
        electricityBottleneckNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityBottleneckNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.BottleneckNotification, value, true);
    }
    private void OnElectricityBuildingBottleneckNotificationToggle(bool value) {
        electricityBuildingBottleneckNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityBuildingBottleneckNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.BuildingBottleneckNotification, value, true);
    }
    private void OnElectricityNotEnoughProductionNotificationToggle(bool value) {
        electricityNotEnoughProductionNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityNotEnoughProductionNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughProductionNotification, value, true);
    }
    private void OnElectricityTransformerNotificationToggle(bool value) {
        electricityTransformerNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityTransformerNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.TransformerNotification, value, true);
    }
    private void OnElectricityNotEnoughConnectedNotificationToggle(bool value) {
        electricityNotEnoughConnectedNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityNotEnoughConnectedNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughConnectedNotification, value, true);
    }
    private void OnElectricityBatteryEmptyNotificationToggle(bool value) {
        electricityBatteryEmptyNotificationBinding.Update(value);
        Setting.Instance.Notification.ElectricityBatteryEmptyNotification = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.BatteryEmptyNotification, value, true);
    }
    private void OnElectricityLowVoltageNotConnectedToggle(bool value) {
        electricityLowVoltageNotConnectedBinding.Update(value);
        Setting.Instance.Notification.ElectricityLowVoltageNotConnected = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.LowVoltageNotConnected, value, true);
    }
    private void OnElectricityHighVoltageNotConnectedToggle(bool value) {
        electricityHighVoltageNotConnectedBinding.Update(value);
        Setting.Instance.Notification.ElectricityHighVoltageNotConnected = value;
        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.HighVoltageNotConnected, value, true);
    }

    #endregion

    #region OnWaterPipeNotificationToggle
    private void OnWaterPipeWaterNotificationToggle(bool value) {
        waterPipeWaterNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeWaterNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterNotification, value, true);
    }
    private void OnWaterPipeDirtyWaterNotificationToggle(bool value) {
        waterPipeDirtyWaterNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeDirtyWaterNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterNotification, value, true);
    }
    private void OnWaterPipeSewageNotificationToggle(bool value) {
        waterPipeSewageNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeSewageNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.SewageNotification, value, true);
    }
    private void OnWaterPipeWaterPipeNotConnectedNotificationToggle(bool value) {
        waterPipeWaterPipeNotConnectedNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterPipeNotConnectedNotification, value, true);
    }
    private void OnWaterPipeSewagePipeNotConnectedNotificationToggle(bool value) {
        waterPipeSewagePipeNotConnectedNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.SewagePipeNotConnectedNotification, value, true);
    }
    private void OnWaterPipeNotEnoughWaterCapacityNotificationToggle(bool value) {
        waterPipeNotEnoughWaterCapacityNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughWaterCapacityNotification, value, true);
    }
    private void OnWaterPipeNotEnoughSewageCapacityNotificationToggle(bool value) {
        waterPipeNotEnoughSewageCapacityNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSewageCapacityNotification, value, true);
    }
    private void OnWaterPipeNotEnoughGroundwaterNotificationToggle(bool value) {
        waterPipeNotEnoughGroundwaterNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughGroundwaterNotification, value, true);
    }
    private void OnWaterPipeNotEnoughSurfaceWaterNotificationToggle(bool value) {
        waterPipeNotEnoughSurfaceWaterNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSurfaceWaterNotification, value, true);
    }
    private void OnWaterPipeDirtyWaterPumpNotificationToggle(bool value) {
        waterPipeDirtyWaterPumpNotificationBinding.Update(value);
        Setting.Instance.Notification.WaterPipeDirtyWaterPumpNotification = value;
        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterPumpNotification, value, true);
    }
    #endregion

    #region OnBuildingNotificationToggle
    private void OnBuildingAbandonedCollapsedNotificationToggle(bool value) {
        buildingAbandonedCollapsedNotificationBinding.Update(value);
        Setting.Instance.Notification.BuildingAbandonedCollapsedNotification = value;
        notificationControllerSystem.EnableBuildingNotification(BuildingNotificationIcon.AbandonedCollapsedNotification, value, true);
    }
    private void OnBuildingAbandonedNotificationToggle(bool value) {
        buildingAbandonedNotificationBinding.Update(value);
        Setting.Instance.Notification.BuildingAbandonedNotification = value;
        notificationControllerSystem.EnableBuildingNotification(BuildingNotificationIcon.AbandonedNotification, value, true);
    }
    private void OnBuildingCondemnedNotificationToggle(bool value) {
        buildingCondemnedNotificationBinding.Update(value);
        Setting.Instance.Notification.BuildingCondemnedNotification = value;
        notificationControllerSystem.EnableBuildingNotification(BuildingNotificationIcon.CondemnedNotification, value, true);
    }
    private void OnBuildingTurnedOffNotificationToggle(bool value) {
        buildingTurnedOffNotificationBinding.Update(value);
        Setting.Instance.Notification.BuildingTurnedOffNotification = value;
        notificationControllerSystem.EnableBuildingNotification(BuildingNotificationIcon.TurnedOffNotification, value, true);
    }
    private void OnBuildingHighRentNotificationToggle(bool value) {
        buildingHighRentNotificationBinding.Update(value);
        Setting.Instance.Notification.BuildingHighRentNotification = value;
        notificationControllerSystem.EnableBuildingNotification(BuildingNotificationIcon.HighRentNotification, value, true);
    }
    #endregion

    #region OnTrafficNotificationToggle
    private void OnTrafficBottleneckNotificationToggle(bool value) {
        trafficBottleneckNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficBottleneckNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.BottleneckNotification, value, true);
    }
    private void OnTrafficDeadEndNotificationToggle(bool value) {
        trafficDeadEndNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficDeadEndNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.DeadEndNotification, value, true);
    }
    private void OnTrafficRoadConnectionNotificationToggle(bool value) {
        trafficRoadConnectionNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficRoadConnectionNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.RoadConnectionNotification, value, true);
    }
    private void OnTrafficTrackConnectionNotificationToggle(bool value) {
        trafficTrackConnectionNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficTrackConnectionNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.TrackConnectionNotification, value, true);
    }
    private void OnTrafficCarConnectionNotificationToggle(bool value) {
        trafficCarConnectionNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficCarConnectionNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.CarConnectionNotification, value, true);
    }
    private void OnTrafficShipConnectionNotificationToggle(bool value) {
        trafficShipConnectionNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficShipConnectionNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.ShipConnectionNotification, value, true);
    }
    private void OnTrafficTrainConnectionNotificationToggle(bool value) {
        trafficTrainConnectionNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficTrainConnectionNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.TrainConnectionNotification, value, true);
    }
    private void OnTrafficPedestrianConnectionNotificationToggle(bool value) {
        trafficPedestrianConnectionNotificationBinding.Update(value);
        Setting.Instance.Notification.TrafficPedestrianConnectionNotification = value;
        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.PedestrianConnectionNotification, value, true);
    }
    #endregion

    private void OnControlPanelBindingToggle(bool value) => panelVisibleBinding.Update(value);

}
