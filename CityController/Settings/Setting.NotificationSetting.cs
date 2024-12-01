namespace CityController.Settings;

public partial class Setting {
    public NotificationSetting Notification { get; set; } = new NotificationSetting();

    public class NotificationSetting {
        public bool ElectricityElectricityNotification { get; set; }
        public bool ElectricityBottleneckNotification { get; set; }
        public bool ElectricityBuildingBottleneckNotification { get; set; }
        public bool ElectricityNotEnoughProductionNotification { get; set; }
        public bool ElectricityTransformerNotification { get; set; }
        public bool ElectricityNotEnoughConnectedNotification { get; set; }
        public bool ElectricityBatteryEmptyNotification { get; set; }
        public bool ElectricityLowVoltageNotConnected { get; set; }
        public bool ElectricityHighVoltageNotConnected { get; set; }


        public bool WaterPipeWaterNotification { get; set; }
        public bool WaterPipeDirtyWaterNotification { get; set; }
        public bool WaterPipeSewageNotification { get; set; }
        public bool WaterPipeWaterPipeNotConnectedNotification { get; set; }
        public bool WaterPipeSewagePipeNotConnectedNotification { get; set; }
        public bool WaterPipeNotEnoughWaterCapacityNotification { get; set; }
        public bool WaterPipeNotEnoughSewageCapacityNotification { get; set; }
        public bool WaterPipeNotEnoughGroundwaterNotification { get; set; }
        public bool WaterPipeNotEnoughSurfaceWaterNotification { get; set; }
        public bool WaterPipeDirtyWaterPumpNotification { get; set; }

        public bool BuildingAbandonedCollapsedNotification { get; set; }
        public bool BuildingAbandonedNotification { get; set; }
        public bool BuildingCondemnedNotification { get; set; }
        public bool BuildingTurnedOffNotification { get; set; }
        public bool BuildingHighRentNotification { get; set; }

        public bool TrafficBottleneckNotification { get; set; }
        public bool TrafficDeadEndNotification { get; set; }
        public bool TrafficRoadConnectionNotification { get; set; }
        public bool TrafficTrackConnectionNotification { get; set; }
        public bool TrafficCarConnectionNotification { get; set; }
        public bool TrafficShipConnectionNotification { get; set; }
        public bool TrafficTrainConnectionNotification { get; set; }
        public bool TrafficPedestrianConnectionNotification { get; set; }
        
        public bool CompanyNoInputsNotification { get; set; }
        public bool CompanyNoCustomersNotification { get; set; }

        public bool WorkProviderUneducatedNotification { get; set; }
        public bool WorkProviderEducatedNotification { get; set; }

        public bool DisasterWeatherDamageNotification { get; set; }
        public bool DisasterWeatherDestroyedNotification { get; set; }
        public bool DisasterWaterDamageNotification { get; set; }
        public bool DisasterWaterDestroyedNotification { get; set; }
        public bool DisasterDestroyedNotification { get; set; }

        public bool FireFireNotification { get; set; }
        public bool FireBurnedDownNotification { get; set; }

        public bool GarbageGarbageNotification { get; set; }
        public bool GarbageFacilityFullNotification { get; set; }

        public bool HealthcareAmbulanceNotification { get; set; }
        public bool HealthcareHearseNotification { get; set; }
        public bool HealthcareFacilityFullNotification { get; set; }

        public bool PoliceTrafficAccidentNotification { get; set; }
        public bool PoliceCrimeSceneNotification { get; set; }

        public bool PollutionAirPollutionNotification { get; set; }
        public bool PollutionNoisePollutionNotification { get; set; }
        public bool PollutionGroundPollutionNotification { get; set; }

        public bool ResourceConsumerNoResourceNotification { get; set; }

        public bool RoutePathfindNotification { get; set; }

        public bool TransportLineVehicleNotification { get; set; }

        public void SetDefaults() {
            ElectricityElectricityNotification = true;
            ElectricityBottleneckNotification = true;
            ElectricityBuildingBottleneckNotification = true;
            ElectricityNotEnoughProductionNotification = true;
            ElectricityTransformerNotification = true;
            ElectricityNotEnoughConnectedNotification = true;
            ElectricityBatteryEmptyNotification = true;
            ElectricityLowVoltageNotConnected = true;
            ElectricityHighVoltageNotConnected = true;

            WaterPipeWaterNotification = true;
            WaterPipeDirtyWaterNotification = true;
            WaterPipeSewageNotification = true;
            WaterPipeWaterPipeNotConnectedNotification = true;
            WaterPipeSewagePipeNotConnectedNotification = true;
            WaterPipeNotEnoughWaterCapacityNotification = true;
            WaterPipeNotEnoughSewageCapacityNotification = true;
            WaterPipeNotEnoughGroundwaterNotification = true;
            WaterPipeNotEnoughSurfaceWaterNotification = true;
            WaterPipeDirtyWaterPumpNotification = true;

            BuildingAbandonedCollapsedNotification = true;
            BuildingAbandonedNotification = true;
            BuildingCondemnedNotification = true;
            BuildingTurnedOffNotification = true;
            BuildingHighRentNotification = true;

            TrafficBottleneckNotification = true;
            TrafficDeadEndNotification = true;
            TrafficRoadConnectionNotification = true;
            TrafficTrackConnectionNotification = true;
            TrafficCarConnectionNotification = true;
            TrafficShipConnectionNotification = true;
            TrafficTrainConnectionNotification = true;
            TrafficPedestrianConnectionNotification = true;
            
            CompanyNoInputsNotification = true;
            CompanyNoCustomersNotification = true;

            WorkProviderUneducatedNotification = true;
            WorkProviderEducatedNotification = true;

            DisasterWeatherDamageNotification = true;
            DisasterWeatherDestroyedNotification = true;
            DisasterWaterDamageNotification = true;
            DisasterWaterDestroyedNotification = true;
            DisasterDestroyedNotification = true;

            FireFireNotification = true;
            FireBurnedDownNotification = true;

            GarbageGarbageNotification = true;
            GarbageFacilityFullNotification = true;

            HealthcareAmbulanceNotification = true;
            HealthcareHearseNotification = true;
            HealthcareFacilityFullNotification = true;

            PoliceTrafficAccidentNotification = true;
            PoliceCrimeSceneNotification = true;

            PollutionAirPollutionNotification = true;
            PollutionNoisePollutionNotification = true;
            PollutionGroundPollutionNotification = true;

            ResourceConsumerNoResourceNotification = true;

            RoutePathfindNotification = true;

            TransportLineVehicleNotification = true;
        }
    }
}

