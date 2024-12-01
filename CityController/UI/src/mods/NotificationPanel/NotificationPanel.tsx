import { useValue } from "cs2/api";
import { game } from "cs2/bindings";
import { useLocalization } from "cs2/l10n";
import { getModule } from "cs2/modding";
import { Button, Panel } from "cs2/ui";
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
    CompanyNoInputsNotificationBinding$, CompanyNoCustomersNotificationBinding$,
    OnCompanyNoInputsNotificationBindingToggle, OnCompanyNoCustomersNotificationBindingToggle, WorkProviderUneducatedNotificationBinding$, WorkProviderEducatedNotificationBinding$, OnWorkProviderUneducatedNotificationBindingToggle, OnWorkProviderEducatedNotificationBindingToggle, DisasterWeatherDamageNotificationBinding$, DisasterWeatherDestroyedNotificationBinding$, DisasterWaterDamageNotificationBinding$, DisasterWaterDestroyedNotificationBinding$, DisasterDestroyedNotificationBinding$, OnDisasterWeatherDamageNotificationBindingToggle, OnDisasterWeatherDestroyedNotificationBindingToggle, OnDisasterWaterDamageNotificationBindingToggle, OnDisasterWaterDestroyedNotificationBindingToggle, OnDisasterDestroyedNotificationBindingToggle, FireFireNotificationBinding$, FireBurnedDownNotificationBinding$, OnFireFireNotificationBindingToggle, OnFireBurnedDownNotificationBindingToggle, GarbageGarbageNotificationBinding$, GarbageFacilityFullNotificationBinding$, OnGarbageGarbageNotificationBindingToggle, OnGarbageFacilityFullNotificationBindingToggle, HealthcareAmbulanceNotificationBinding$, HealthcareHearseNotificationBinding$, HealthcareFacilityFullNotificationBinding$, OnHealthcareAmbulanceNotificationBindingToggle, OnHealthcareHearseNotificationBindingToggle, OnHealthcareFacilityFullNotificationBindingToggle, PoliceTrafficAccidentNotificationBinding$, PoliceCrimeSceneNotificationBinding$, OnPoliceTrafficAccidentNotificationBindingToggle, OnPoliceCrimeSceneNotificationBindingToggle,
    PollutionAirPollutionNotificationBinding$, PollutionNoisePollutionNotificationBinding$, PollutionGroundPollutionNotificationBinding$, OnPollutionAirPollutionNotificationBindingToggle, OnPollutionNoisePollutionNotificationBindingToggle, OnPollutionGroundPollutionNotificationBindingToggle, ResourceConsumerNoResourceNotificationBinding$, OnResourceConsumerNoResourceNotificationBindingToggle, RoutePathfindNotificationBinding$, OnRoutePathfindNotificationBindingToggle, TransportLineVehicleNotificationBinding$, OnTransportLineVehicleNotificationBindingToggle,
    controlPanelEnabled$
} from "../Bindings/Bindings";
import { Divider } from "../Divider/Divider";
import { InfoCheckbox } from "../InfoCheckbox/InfoCheckbox";
import { InfoPanel } from "../InfoPanel/InfoPanel";
import styles from "../NotificationPanel/NotificationPanel.module.scss";
import { VanillaComponentResolver } from "../VanillaComponentResolver/VanillaComponentResolver";


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
    const companyNoInputsNotificationBinding = useValue(CompanyNoInputsNotificationBinding$);
    const companyNoCustomersNotificationBinding = useValue(CompanyNoCustomersNotificationBinding$);
    const workProviderUneducatedNotificationBinding = useValue(WorkProviderUneducatedNotificationBinding$);
    const workProviderEducatedNotificationBinding = useValue(WorkProviderEducatedNotificationBinding$);
    const disasterWeatherDamageNotificationBinding = useValue(DisasterWeatherDamageNotificationBinding$);
    const disasterWeatherDestroyedNotificationBinding = useValue(DisasterWeatherDestroyedNotificationBinding$);
    const disasterWaterDamageNotificationBinding = useValue(DisasterWaterDamageNotificationBinding$);
    const disasterWaterDestroyedNotificationBinding = useValue(DisasterWaterDestroyedNotificationBinding$);
    const disasterDestroyedNotificationBinding = useValue(DisasterDestroyedNotificationBinding$);
    const fireFireNotificationBinding = useValue(FireFireNotificationBinding$);
    const fireBurnedDownNotificationBinding = useValue(FireBurnedDownNotificationBinding$);
    const garbageGarbageNotificationBinding = useValue(GarbageGarbageNotificationBinding$);
    const garbageFacilityFullNotificationBinding = useValue(GarbageFacilityFullNotificationBinding$);
    const healthcareAmbulanceNotificationBinding = useValue(HealthcareAmbulanceNotificationBinding$);
    const healthcareHearseNotificationBinding = useValue(HealthcareHearseNotificationBinding$);
    const healthcareFacilityFullNotificationBinding = useValue(HealthcareFacilityFullNotificationBinding$);
    const policeTrafficAccidentNotificationBinding = useValue(PoliceTrafficAccidentNotificationBinding$);
    const policeCrimeSceneNotificationBinding = useValue(PoliceCrimeSceneNotificationBinding$);
    const pollutionAirPollutionNotificationBinding = useValue(PollutionAirPollutionNotificationBinding$);
    const pollutionNoisePollutionNotificationBinding = useValue(PollutionNoisePollutionNotificationBinding$);
    const pollutionGroundPollutionNotificationBinding = useValue(PollutionGroundPollutionNotificationBinding$);
    const resourceConsumerNoResourceNotificationBinding = useValue(ResourceConsumerNoResourceNotificationBinding$);
    const routePathfindNotificationBinding = useValue(RoutePathfindNotificationBinding$);
    const transportLineVehicleNotificationBinding = useValue(TransportLineVehicleNotificationBinding$);
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
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Company")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoInputs.svg"
                        label={localize("CompanyNoInputsNotification")}
                        isChecked={companyNoInputsNotificationBinding}
                        onToggle={(value) => OnCompanyNoInputsNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoCustomers.svg"
                        label={localize("CompanyNoCustomersNotification")}
                        isChecked={companyNoCustomersNotificationBinding}
                        onToggle={(value) => OnCompanyNoCustomersNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("WorkProvider")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoWorkers.svg"
                        label={localize("WorkProviderUneducatedNotification")}
                        isChecked={workProviderUneducatedNotificationBinding}
                        onToggle={(value) => OnWorkProviderUneducatedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoEducatedWorkers.svg"
                        label={localize("WorkProviderEducatedNotification")}
                        isChecked={workProviderEducatedNotificationBinding}
                        onToggle={(value) => OnWorkProviderEducatedNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Disaster")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/WeatherDamage.svg"
                        label={localize("DisasterWeatherDamageNotification")}
                        isChecked={disasterWeatherDamageNotificationBinding}
                        onToggle={(value) => OnDisasterWeatherDamageNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/WeatherDestroyed.svg"
                        label={localize("DisasterWeatherDestroyedNotification")}
                        isChecked={disasterWeatherDestroyedNotificationBinding}
                        onToggle={(value) => OnDisasterWeatherDestroyedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/WaterDamage.svg"
                        label={localize("DisasterWaterDamageNotification")}
                        isChecked={disasterWaterDamageNotificationBinding}
                        onToggle={(value) => OnDisasterWaterDamageNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/WaterDestroyed.svg"
                        label={localize("DisasterWaterDestroyedNotification")}
                        isChecked={disasterWaterDestroyedNotificationBinding}
                        onToggle={(value) => OnDisasterWaterDestroyedNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/Destroyed.svg"
                        label={localize("DisasterDestroyedNotification")}
                        isChecked={disasterDestroyedNotificationBinding}
                        onToggle={(value) => OnDisasterDestroyedNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Fire")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/BuildingOnFire.svg"
                        label={localize("FireFireNotification")}
                        isChecked={fireFireNotificationBinding}
                        onToggle={(value) => OnFireFireNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/BurnedDown.svg"
                        label={localize("FireBurnedDownNotification")}
                        isChecked={fireBurnedDownNotificationBinding}
                        onToggle={(value) => OnFireBurnedDownNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Garbage")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/TooMuchGarbage.svg"
                        label={localize("GarbageGarbageNotification")}
                        isChecked={garbageGarbageNotificationBinding}
                        onToggle={(value) => OnGarbageGarbageNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/FacilityFull.svg"
                        label={localize("GarbageFacilityFullNotification")}
                        isChecked={garbageFacilityFullNotificationBinding}
                        onToggle={(value) => OnGarbageFacilityFullNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Healthcare")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/MedicalEmergency.svg"
                        label={localize("HealthcareAmbulanceNotification")}
                        isChecked={healthcareAmbulanceNotificationBinding}
                        onToggle={(value) => OnHealthcareAmbulanceNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/HearseServiceNeeded.svg"
                        label={localize("HealthcareHearseNotification")}
                        isChecked={healthcareHearseNotificationBinding}
                        onToggle={(value) => OnHealthcareHearseNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/FacilityFull.svg"
                        label={localize("HealthcareFacilityFullNotification")}
                        isChecked={healthcareFacilityFullNotificationBinding}
                        onToggle={(value) => OnHealthcareFacilityFullNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Police")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/TrafficAccident.svg"
                        label={localize("PoliceTrafficAccidentNotification")}
                        isChecked={policeTrafficAccidentNotificationBinding}
                        onToggle={(value) => OnPoliceTrafficAccidentNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/CrimeScene.svg"
                        label={localize("PoliceCrimeSceneNotification")}
                        isChecked={policeCrimeSceneNotificationBinding}
                        onToggle={(value) => OnPoliceCrimeSceneNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Pollution")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/AirPollution.svg"
                        label={localize("PollutionAirPollutionNotification")}
                        isChecked={pollutionAirPollutionNotificationBinding}
                        onToggle={(value) => OnPollutionAirPollutionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoisePollution.svg"
                        label={localize("PollutionNoisePollutionNotification")}
                        isChecked={pollutionNoisePollutionNotificationBinding}
                        onToggle={(value) => OnPollutionNoisePollutionNotificationBindingToggle(value)}
                        style={{ marginBottom: "5rem" }}
                    ></InfoCheckbox>
                    <InfoCheckbox
                        image="media/Game/Notifications/PollutedSoil.svg"
                        label={localize("PollutionGroundPollutionNotification")}
                        isChecked={pollutionGroundPollutionNotificationBinding}
                        onToggle={(value) => OnPollutionGroundPollutionNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("ResourceConsumer")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/NotEnoughIndustrialGoods.svg"
                        label={localize("ResourceConsumerNoResourceNotification")}
                        isChecked={resourceConsumerNoResourceNotificationBinding}
                        onToggle={(value) => OnResourceConsumerNoResourceNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("Route")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/PathfindFailed.svg"
                        label={localize("RoutePathfindNotification")}
                        isChecked={routePathfindNotificationBinding}
                        onToggle={(value) => OnRoutePathfindNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>

                <Divider></Divider>

                <InfoPanel title={localize("TransportLine")}>
                    <InfoCheckbox
                        image="media/Game/Notifications/NoVehicles.svg"
                        label={localize("TransportLineVehicleNotification")}
                        isChecked={transportLineVehicleNotificationBinding}
                        onToggle={(value) => OnTransportLineVehicleNotificationBindingToggle(value)}
                    ></InfoCheckbox>
                </InfoPanel>
            </Panel>

            

        </>
    )
}