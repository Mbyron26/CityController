import { bindValue, trigger } from "cs2/api";
import mod from "../../../mod.json";

export const controlPanelEnabled$ = bindValue<boolean>(mod.id, "ControlPanelEnabled", false);

export const ElectricityElectricityNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityElectricityNotification");
export const ElectricityBottleneckNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityBottleneckNotification");
export const ElectricityBuildingBottleneckNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityBuildingBottleneckNotification");
export const ElectricityNotEnoughProductionNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityNotEnoughProductionNotification");
export const ElectricityTransformerNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityTransformerNotification");
export const ElectricityNotEnoughConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityNotEnoughConnectedNotification");
export const ElectricityBatteryEmptyNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityBatteryEmptyNotification");
export const ElectricityLowVoltageNotConnectedBinding$ = bindValue<boolean>(mod.id, "ElectricityLowVoltageNotConnected");
export const ElectricityHighVoltageNotConnectedBinding$ = bindValue<boolean>(mod.id, "ElectricityHighVoltageNotConnected");

export const WaterPipeWaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeWaterNotification");
export const WaterPipeDirtyWaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeDirtyWaterNotification");
export const WaterPipeSewageNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeSewageNotification");
export const WaterPipeWaterPipeNotConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeWaterPipeNotConnectedNotification");
export const WaterPipeSewagePipeNotConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeSewagePipeNotConnectedNotification");
export const WaterPipeNotEnoughWaterCapacityNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughWaterCapacityNotification");
export const WaterPipeNotEnoughSewageCapacityNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughSewageCapacityNotification");
export const WaterPipeNotEnoughGroundwaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughGroundwaterNotification");
export const WaterPipeNotEnoughSurfaceWaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughSurfaceWaterNotification");
export const WaterPipeDirtyWaterPumpNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeDirtyWaterPumpNotification");

export const BuildingAbandonedCollapsedNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingAbandonedCollapsedNotification");
export const BuildingAbandonedNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingAbandonedNotification");
export const BuildingCondemnedNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingCondemnedNotification");
export const BuildingTurnedOffNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingTurnedOffNotification");
export const BuildingHighRentNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingHighRentNotification");

export const TrafficBottleneckNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficBottleneckNotification");
export const TrafficDeadEndNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficDeadEndNotification");
export const TrafficRoadConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficRoadConnectionNotification");
export const TrafficTrackConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficTrackConnectionNotification");
export const TrafficCarConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficCarConnectionNotification");
export const TrafficShipConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficShipConnectionNotification");
export const TrafficTrainConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficTrainConnectionNotification");
export const TrafficPedestrianConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficPedestrianConnectionNotification");


export const OnControlPanelBindingToggle = (enable: boolean) => trigger(mod.id, "ControlPanelEnabled", enable);

export const OnElectricityElectricityNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityElectricityNotification", enable);
export const OnElectricityBottleneckNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityBottleneckNotification", enable);
export const OnElectricityBuildingBottleneckNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityBuildingBottleneckNotification", enable);
export const OnElectricityNotEnoughProductionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityNotEnoughProductionNotification", enable);
export const OnElectricityTransformerNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityTransformerNotification", enable);
export const OnElectricityNotEnoughConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityNotEnoughConnectedNotification", enable);
export const OnElectricityBatteryEmptyNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityBatteryEmptyNotification", enable);
export const OnElectricityLowVoltageNotConnectedBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityLowVoltageNotConnected", enable);
export const OnElectricityHighVoltageNotConnectedBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityHighVoltageNotConnected", enable);

export const OnWaterPipeWaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeWaterNotification", enable);
export const OnWaterPipeDirtyWaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeDirtyWaterNotification", enable);
export const OnWaterPipeSewageNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeSewageNotification", enable);
export const OnWaterPipeWaterPipeNotConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeWaterPipeNotConnectedNotification", enable);
export const OnWaterPipeSewagePipeNotConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeSewagePipeNotConnectedNotification", enable);
export const OnWaterPipeNotEnoughWaterCapacityNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughWaterCapacityNotification", enable);
export const OnWaterPipeNotEnoughSewageCapacityNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughSewageCapacityNotification", enable);
export const OnWaterPipeNotEnoughGroundwaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughGroundwaterNotification", enable);
export const OnWaterPipeNotEnoughSurfaceWaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughSurfaceWaterNotification", enable);
export const OnWaterPipeDirtyWaterPumpNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeDirtyWaterPumpNotification", enable);

export const OnBuildingAbandonedCollapsedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingAbandonedCollapsedNotification", enable);
export const OnBuildingAbandonedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingAbandonedNotification", enable);
export const OnBuildingCondemnedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingCondemnedNotification", enable);
export const OnBuildingTurnedOffNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingTurnedOffNotification", enable);
export const OnBuildingHighRentNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingHighRentNotification", enable);

export const OnTrafficBottleneckNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficBottleneckNotification", enable);
export const OnTrafficDeadEndNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficDeadEndNotification", enable);
export const OnTrafficRoadConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficRoadConnectionNotification", enable);
export const OnTrafficTrackConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficTrackConnectionNotification", enable);
export const OnTrafficCarConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficCarConnectionNotification", enable);
export const OnTrafficShipConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficShipConnectionNotification", enable);
export const OnTrafficTrainConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficTrainConnectionNotification", enable);
export const OnTrafficPedestrianConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficPedestrianConnectionNotification", enable);