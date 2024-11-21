import { useValue } from "cs2/api";
import { Theme, game } from "cs2/bindings";
import { useLocalization } from "cs2/l10n";
import { getModule } from "cs2/modding";
import { Button, Panel, Scrollable, Tooltip } from "cs2/ui";
import {
    BuildingAbandonedCollapsedNotificationBinding$, BuildingAbandonedNotificationBinding$, BuildingCondemnedNotificationBinding$,
    BuildingHighRentNotificationBinding$,
    BuildingTurnedOffNotificationBinding$,
    ElectricityBatteryEmptyNotificationBinding$,
    ElectricityBottleneckNotificationBinding$, ElectricityBuildingBottleneckNotificationBinding$,
    ElectricityElectricityNotificationBinding$,
    ElectricityHighVoltageNotConnectedBinding$, ElectricityLowVoltageNotConnectedBinding$,
    ElectricityNotEnoughConnectedNotificationBinding$,
    ElectricityNotEnoughProductionNotificationBinding$, ElectricityTransformerNotificationBinding$,
    OnBuildingAbandonedCollapsedNotificationBindingToggle, OnBuildingAbandonedNotificationBindingToggle, OnBuildingCondemnedNotificationBindingToggle,
    OnBuildingHighRentNotificationBindingToggle,
    OnBuildingTurnedOffNotificationBindingToggle,
    OnControlPanelBindingToggle,
    OnElectricityBatteryEmptyNotificationBindingToggle,
    OnElectricityBottleneckNotificationBindingToggle, OnElectricityBuildingBottleneckNotificationBindingToggle,
    OnElectricityElectricityNotificationBindingToggle,
    OnElectricityHighVoltageNotConnectedBindingToggle, OnElectricityLowVoltageNotConnectedBindingToggle,
    OnElectricityNotEnoughConnectedNotificationBindingToggle,
    OnElectricityNotEnoughProductionNotificationBindingToggle, OnElectricityTransformerNotificationBindingToggle,
    OnTrafficBottleneckNotificationBindingToggle,
    OnTrafficCarConnectionNotificationBindingToggle,
    OnTrafficDeadEndNotificationBindingToggle,
    OnTrafficPedestrianConnectionNotificationBindingToggle,
    OnTrafficRoadConnectionNotificationBindingToggle,
    OnTrafficShipConnectionNotificationBindingToggle,
    OnTrafficTrackConnectionNotificationBindingToggle,
    OnTrafficTrainConnectionNotificationBindingToggle,
    OnWaterPipeDirtyWaterNotificationBindingToggle,
    OnWaterPipeDirtyWaterPumpNotificationBindingToggle,
    OnWaterPipeNotEnoughGroundwaterNotificationBindingToggle,
    OnWaterPipeNotEnoughSewageCapacityNotificationBindingToggle,
    OnWaterPipeNotEnoughSurfaceWaterNotificationBindingToggle,
    OnWaterPipeNotEnoughWaterCapacityNotificationBindingToggle,
    OnWaterPipeSewageNotificationBindingToggle,
    OnWaterPipeSewagePipeNotConnectedNotificationBindingToggle,
    OnWaterPipeWaterNotificationBindingToggle,
    OnWaterPipeWaterPipeNotConnectedNotificationBindingToggle,
    TrafficBottleneckNotificationBinding$,
    TrafficCarConnectionNotificationBinding$,
    TrafficDeadEndNotificationBinding$,
    TrafficPedestrianConnectionNotificationBinding$,
    TrafficRoadConnectionNotificationBinding$,
    TrafficShipConnectionNotificationBinding$,
    TrafficTrackConnectionNotificationBinding$,
    TrafficTrainConnectionNotificationBinding$,
    WaterPipeDirtyWaterNotificationBinding$,
    WaterPipeDirtyWaterPumpNotificationBinding$,
    WaterPipeNotEnoughGroundwaterNotificationBinding$,
    WaterPipeNotEnoughSewageCapacityNotificationBinding$,
    WaterPipeNotEnoughSurfaceWaterNotificationBinding$,
    WaterPipeNotEnoughWaterCapacityNotificationBinding$,
    WaterPipeSewageNotificationBinding$,
    WaterPipeSewagePipeNotConnectedNotificationBinding$,
    WaterPipeWaterNotificationBinding$,
    WaterPipeWaterPipeNotConnectedNotificationBinding$,
    controlPanelEnabled$,
} from "../Bindings/Bindings";
import { InfoCheckbox } from "../InfoCheckbox/InfoCheckbox";
import { VanillaComponentResolver } from "../VanillaComponentResolver/VanillaComponentResolver";
import styles from "../NotificationPanel/NotificationPanel.module.scss";
import { InfoPanel } from "../InfoPanel/InfoPanel";
import { Divider } from "../Divider/Divider";


const modIconSrc = "coui://ui-mods/images/CityControllerIcon_colored.svg";
const roundButtonHighlightStyle = getModule("game-ui/common/input/button/themes/round-highlight-button.module.scss", "classes");


export const NotificationPanel = () => {
    const { translate } = useLocalization();
    const localize = (localeId: string): string => translate(`CityController.UI[${localeId}]`) ?? "Default Value";

    const showPanel = useValue(controlPanelEnabled$);
    const isPhotoMode = useValue(game.activeGamePanel$)?.__Type == game.GamePanelType.PhotoMode;
    const electricityElectricityNotificationBinding = useValue(ElectricityElectricityNotificationBinding$);
    const electricityBottleneckNotificationBinding = useValue(ElectricityBottleneckNotificationBinding$);
    const electricityBuildingBottleneckNotificationBinding = useValue(ElectricityBuildingBottleneckNotificationBinding$);
    const electricityNotEnoughProductionNotificationBinding = useValue(ElectricityNotEnoughProductionNotificationBinding$);
    const electricityTransformerNotificationBinding = useValue(ElectricityTransformerNotificationBinding$);
    const electricityNotEnoughConnectedNotificationBinding = useValue(ElectricityNotEnoughConnectedNotificationBinding$);
    const electricityBatteryEmptyNotificationBinding = useValue(ElectricityBatteryEmptyNotificationBinding$);
    const electricityLowVoltageNotConnectedBinding = useValue(ElectricityLowVoltageNotConnectedBinding$);
    const electricityHighVoltageNotConnectedBinding = useValue(ElectricityHighVoltageNotConnectedBinding$);

    const waterPipeWaterNotificationBinding = useValue(WaterPipeWaterNotificationBinding$);
    const waterPipeDirtyWaterNotificationBinding = useValue(WaterPipeDirtyWaterNotificationBinding$);
    const waterPipeSewageNotificationBinding = useValue(WaterPipeSewageNotificationBinding$);
    const waterPipeWaterPipeNotConnectedNotificationBinding = useValue(WaterPipeWaterPipeNotConnectedNotificationBinding$);
    const waterPipeSewagePipeNotConnectedNotificationBinding = useValue(WaterPipeSewagePipeNotConnectedNotificationBinding$);
    const waterPipeNotEnoughWaterCapacityNotificationBinding = useValue(WaterPipeNotEnoughWaterCapacityNotificationBinding$);
    const waterPipeNotEnoughSewageCapacityNotificationBinding = useValue(WaterPipeNotEnoughSewageCapacityNotificationBinding$);
    const waterPipeNotEnoughGroundwaterNotificationBinding = useValue(WaterPipeNotEnoughGroundwaterNotificationBinding$);
    const waterPipeNotEnoughSurfaceWaterNotificationBinding = useValue(WaterPipeNotEnoughSurfaceWaterNotificationBinding$);
    const waterPipeDirtyWaterPumpNotificationBinding = useValue(WaterPipeDirtyWaterPumpNotificationBinding$);

    const buildingAbandonedCollapsedNotificationBinding = useValue(BuildingAbandonedCollapsedNotificationBinding$);
    const buildingAbandonedNotificationBinding = useValue(BuildingAbandonedNotificationBinding$);
    const buildingCondemnedNotificationBinding = useValue(BuildingCondemnedNotificationBinding$);
    const buildingTurnedOffNotificationBinding = useValue(BuildingTurnedOffNotificationBinding$);
    const buildingHighRentNotificationBinding = useValue(BuildingHighRentNotificationBinding$);

    const trafficBottleneckNotificationBinding = useValue(TrafficBottleneckNotificationBinding$);
    const trafficDeadEndNotificationBinding = useValue(TrafficDeadEndNotificationBinding$);
    const trafficRoadConnectionNotificationBinding = useValue(TrafficRoadConnectionNotificationBinding$);
    const trafficTrackConnectionNotificationBinding = useValue(TrafficTrackConnectionNotificationBinding$);
    const trafficCarConnectionNotificationBinding = useValue(TrafficCarConnectionNotificationBinding$);
    const trafficShipConnectionNotificationBinding = useValue(TrafficShipConnectionNotificationBinding$);
    const trafficTrainConnectionNotificationBinding = useValue(TrafficTrainConnectionNotificationBinding$);
    const trafficPedestrianConnectionNotificationBinding = useValue(TrafficPedestrianConnectionNotificationBinding$);

    if (isPhotoMode || !showPanel) {
        return null;
    }

    return (
        <>
            <Panel className={styles.panel}
                header={
                    <div className={styles.header}>
                        <img src={modIconSrc} className={styles.headerModIcon} />
                        <div className={styles.headerModName}>CITY CONTROLLER</div>
                        <Button className={roundButtonHighlightStyle.button + ' ' + styles.headerCloseButton}
                            variant="icon"
                            onClick={() => { OnControlPanelBindingToggle(!showPanel) }}
                            focusKey={VanillaComponentResolver.instance.FOCUS_DISABLED}>
                            <img src="coui://uil/Standard/XClose.svg"></img>
                        </Button>
                    </div>
                }>

                <div style={{ padding: "10rem", fontSize: "var(--fontSizeS)" }}>{localize("NotificationIconShowOrHide")}</div>

                <InfoPanel title={localize("Electricity")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/NotEnoughElectricity.svg"
                        label={localize("ElectricityElectricityNotification")}
                        isChecked={electricityElectricityNotificationBinding}
                        onToggle={(value) => OnElectricityElectricityNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/ElectricityBottleneck.svg"
                        label={localize("ElectricityBottleneckNotification")}
                        isChecked={electricityBottleneckNotificationBinding}
                        onToggle={(value) => OnElectricityBottleneckNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/BadServiceElectricity.svg"
                        label={localize("ElectricityBuildingBottleneckNotification")}
                        isChecked={electricityBuildingBottleneckNotificationBinding}
                        onToggle={(value) => OnElectricityBuildingBottleneckNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/LowProductionElectricity.svg"
                        label={localize("ElectricityNotEnoughProductionNotification")}
                        isChecked={electricityNotEnoughProductionNotificationBinding}
                        onToggle={(value) => OnElectricityNotEnoughProductionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/OutOfCapacityElectricity.svg"
                        label={localize("ElectricityTransformerNotification")}
                        isChecked={electricityTransformerNotificationBinding}
                        onToggle={(value) => OnElectricityTransformerNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NotEnoughOutputLinesConnected.svg"
                        label={localize("ElectricityNotEnoughConnectedNotification")}
                        isChecked={electricityNotEnoughConnectedNotificationBinding}
                        onToggle={(value) => OnElectricityNotEnoughConnectedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/BatteryEmpty.svg"
                        label={localize("ElectricityBatteryEmptyNotification")}
                        isChecked={electricityBatteryEmptyNotificationBinding}
                        onToggle={(value) => OnElectricityBatteryEmptyNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/PowerlineDisconnectedLow.svg"
                        label={localize("ElectricityLowVoltageNotConnected")}
                        isChecked={electricityLowVoltageNotConnectedBinding}
                        onToggle={(value) => OnElectricityLowVoltageNotConnectedBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/PowerlineDisconnected.svg"
                        label={localize("ElectricityHighVoltageNotConnected")}
                        isChecked={electricityHighVoltageNotConnectedBinding}
                        onToggle={(value) => OnElectricityHighVoltageNotConnectedBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("WaterPipe")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoRunningWater.svg"
                        label={localize("WaterPipeWaterNotification")}
                        isChecked={waterPipeWaterNotificationBinding}
                        onToggle={(value) => OnWaterPipeWaterNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/ContaminatedWaterPumped.svg"
                        label={localize("WaterPipeDirtyWaterNotification")}
                        isChecked={waterPipeDirtyWaterNotificationBinding}
                        onToggle={(value) => OnWaterPipeDirtyWaterNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/Sewage.svg"
                        label={localize("WaterPipeSewageNotification")}
                        isChecked={waterPipeSewageNotificationBinding}
                        onToggle={(value) => OnWaterPipeSewageNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/WaterPipeDisconnected.svg"
                        label={localize("WaterPipeWaterPipeNotConnectedNotification")}
                        isChecked={waterPipeWaterPipeNotConnectedNotificationBinding}
                        onToggle={(value) => OnWaterPipeWaterPipeNotConnectedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/SewagePipeDisconnected.svg"
                        label={localize("WaterPipeSewagePipeNotConnectedNotification")}
                        isChecked={waterPipeSewagePipeNotConnectedNotificationBinding}
                        onToggle={(value) => OnWaterPipeSewagePipeNotConnectedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/WaterNotEnoughProduction.svg"
                        label={localize("WaterPipeNotEnoughWaterCapacityNotification")}
                        isChecked={waterPipeNotEnoughWaterCapacityNotificationBinding}
                        onToggle={(value) => OnWaterPipeNotEnoughWaterCapacityNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/SewageNotEnoughProduction.svg"
                        label={localize("WaterPipeNotEnoughSewageCapacityNotification")}
                        isChecked={waterPipeNotEnoughSewageCapacityNotificationBinding}
                        onToggle={(value) => OnWaterPipeNotEnoughSewageCapacityNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/GroundwaterLevelLow.svg"
                        label={localize("WaterPipeNotEnoughGroundwaterNotification")}
                        isChecked={waterPipeNotEnoughGroundwaterNotificationBinding}
                        onToggle={(value) => OnWaterPipeNotEnoughGroundwaterNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/SurfaceWaterLevelLow.svg"
                        label={localize("WaterPipeNotEnoughSurfaceWaterNotification")}
                        isChecked={waterPipeNotEnoughSurfaceWaterNotificationBinding}
                        onToggle={(value) => OnWaterPipeNotEnoughSurfaceWaterNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/DirtyWaterPump.svg"
                        label={localize("WaterPipeDirtyWaterPumpNotification")}
                        isChecked={waterPipeDirtyWaterPumpNotificationBinding}
                        onToggle={(value) => OnWaterPipeDirtyWaterPumpNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Building")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/BuildingCollapsed.svg"
                        label={localize("BuildingAbandonedCollapsedNotification")}
                        isChecked={buildingAbandonedCollapsedNotificationBinding}
                        onToggle={(value) => OnBuildingAbandonedCollapsedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/BuildingAbandoned.svg"
                        label={localize("BuildingAbandonedNotification")}
                        isChecked={buildingAbandonedNotificationBinding}
                        onToggle={(value) => OnBuildingAbandonedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/BuildingCondemned.svg"
                        label={localize("BuildingCondemnedNotification")}
                        isChecked={buildingCondemnedNotificationBinding}
                        onToggle={(value) => OnBuildingCondemnedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/TurnedOff.svg"
                        label={localize("BuildingTurnedOffNotification")}
                        isChecked={buildingTurnedOffNotificationBinding}
                        onToggle={(value) => OnBuildingTurnedOffNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/RentTooHigh.svg"
                        label={localize("BuildingHighRentNotification")}
                        isChecked={buildingHighRentNotificationBinding}
                        onToggle={(value) => OnBuildingHighRentNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Traffic")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/TrafficBottleneck.svg"
                        label={localize("TrafficBottleneckNotification")}
                        isChecked={trafficBottleneckNotificationBinding}
                        onToggle={(value) => OnTrafficBottleneckNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/DeadEnd.svg"
                        label={localize("TrafficDeadEndNotification")}
                        isChecked={trafficDeadEndNotificationBinding}
                        onToggle={(value) => OnTrafficDeadEndNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/RoadNotConnected.svg"
                        label={localize("TrafficRoadConnectionNotification")}
                        isChecked={trafficRoadConnectionNotificationBinding}
                        onToggle={(value) => OnTrafficRoadConnectionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/TrackNotConnected.svg"
                        label={localize("TrafficTrackConnectionNotification")}
                        isChecked={trafficTrackConnectionNotificationBinding}
                        onToggle={(value) => OnTrafficTrackConnectionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoCarAccess.svg"
                        label={localize("TrafficCarConnectionNotification")}
                        isChecked={trafficCarConnectionNotificationBinding}
                        onToggle={(value) => OnTrafficCarConnectionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoBoatAccess.svg"
                        label={localize("TrafficShipConnectionNotification")}
                        isChecked={trafficShipConnectionNotificationBinding}
                        onToggle={(value) => OnTrafficShipConnectionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoTrainAccess.svg"
                        label={localize("TrafficTrainConnectionNotification")}
                        isChecked={trafficTrainConnectionNotificationBinding}
                        onToggle={(value) => OnTrafficTrainConnectionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoPedestrianAccess.svg"
                        label={localize("TrafficPedestrianConnectionNotification")}
                        isChecked={trafficPedestrianConnectionNotificationBinding}
                        onToggle={(value) => OnTrafficPedestrianConnectionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                </InfoPanel>

            </Panel>

        </>
    )
}