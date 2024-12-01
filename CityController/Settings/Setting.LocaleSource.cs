using System.Collections.Generic;
using System.Linq;

namespace CityController.Settings;

public partial class Setting {
    protected override void CreateLocaleSource() {
        base.CreateLocaleSource();
        AddLocaleSource(GetSettingsLocaleID(), "City Controller");
        AddLocaleSource(GetOptionGroupLocaleID(Achievements), "Achievements");
        AddLocaleSource(GetOptionGroupLocaleID(Money), "Money");
        AddLocaleSource(GetOptionGroupLocaleID(Milestone), "Milestone");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AchievementsEnabled)), "Enable Achievements");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AchievementsEnabled)), "Allows Achievements system to be activated even when Mods ars enabled, Unlock All or Unlimited Money is enabled.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AutomaticAddMoney)), "Automatic Add Money");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AutomaticAddMoney)), $"Select to enable automatic add money mode.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(ManualMoneyAmount)), "Manual Money Amount");
        AddLocaleSource(GetOptionDescLocaleID(nameof(ManualMoneyAmount)), $"Set this value to manually add/subtract money amounts.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AutomaticAddMoneyThreshold)), "Automatic Add Money Threshold");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AutomaticAddMoneyThreshold)), $"Set this value to automatically add money when money falls below this set threshold.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AutomaticAddMoneyAmount)), "Automatic Add Money Amount");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AutomaticAddMoneyAmount)), $"Set the amount of money that will be automatically added when money falls below the threshold.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(MoneyTransfer)), "Money Transfer");
        AddLocaleSource(GetOptionDescLocaleID(nameof(MoneyTransfer)), $"Allows you to switch Unlimited Money to Limited Money in the game, this options only available in-game");
        AddLocaleSource(GetOptionWarningLocaleID(nameof(MoneyTransfer)), "Are you sure you want to convert Unlimited Money to Limited Money? This operation is not reversible!");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(CustomMilestone)), "Custom Milestone");
        AddLocaleSource(GetOptionDescLocaleID(nameof(CustomMilestone)), "Enable this option to customize the start milestone, and needs to be seton the main menu page before entering the game.");
        AddLocaleSource(new Dictionary<string, string>() {
            { GetOptionLabelLocaleID(nameof(MilestoneLevel)), "Milestone" },
            { GetOptionDescLocaleID(nameof(MilestoneLevel)), "Select any milestone level to unlock before starting game, and needs to be set on the main menu page before entering the game." },
            { GetOptionLabelLocaleID(nameof(AddMoneyKeyboardBinding)), "Add Money" },
            { GetOptionDescLocaleID(nameof(AddMoneyKeyboardBinding)), $"Hotkeys for adding money within the game." },
            { GetBindingKeyLocaleID(AddMoneyAction), "Add Money" },
            { GetOptionLabelLocaleID(nameof(SubtractMoneyKeyboardBinding)), "Subtract Money" },
            { GetOptionDescLocaleID(nameof(SubtractMoneyKeyboardBinding)), $"Hotkeys for subtracting money within the game." },
            { GetBindingKeyLocaleID(SubtractMoneyAction), "Subtract Money" },
            { GetOptionLabelLocaleID(nameof(InitialMoney)), "Initial Money" },
            { GetOptionDescLocaleID(nameof(InitialMoney)), $"Set the game initial money. This option is valid for New/Loaded games and only takes effect once. Note that this option can only be changed when not in game." },
            { GetOptionLocaleID("GameDefault"), "Game Default" },
        });
        AddLocaleSource(Milestones.ToDictionary(milestone => GetOptionLocaleID(milestone), milestone => milestone));
        AddLocaleSource(new Dictionary<string, string>() {
            { GetUILocaleID("NotificationIconShowOrHide"), "Notification icon show/hide"},
            { GetUILocaleID("Electricity"), "ELECTRICITY"},
            { GetUILocaleID("ElectricityElectricityNotification"), "Not enough electricity"},
            { GetUILocaleID("ElectricityBottleneckNotification"), "Electricity bottleneck"},
            { GetUILocaleID("ElectricityBuildingBottleneckNotification"), "Poor electricity flow"},
            { GetUILocaleID("ElectricityNotEnoughProductionNotification"), "Power station overload"},
            { GetUILocaleID("ElectricityTransformerNotification"), "Transformer overload"},
            { GetUILocaleID("ElectricityNotEnoughConnectedNotification"), "Not enough output lines connected"},
            { GetUILocaleID("ElectricityBatteryEmptyNotification"), "Battery depleted"},
            { GetUILocaleID("ElectricityLowVoltageNotConnected"), "Electric Cable not connected"},
            { GetUILocaleID("ElectricityHighVoltageNotConnected"), "Power Line not connected"},
            { GetUILocaleID("WaterPipe"), "WATER RPIPE"},
            { GetUILocaleID("WaterPipeWaterNotification"), "Not enough water"},
            { GetUILocaleID("WaterPipeDirtyWaterNotification"), "Water pump polluted"},
            { GetUILocaleID("WaterPipeSewageNotification"), "Backed up sewer"},
            { GetUILocaleID("WaterPipeWaterPipeNotConnectedNotification"), "Water Pipe not connected"},
            { GetUILocaleID("WaterPipeSewagePipeNotConnectedNotification"), "Sewage Pipe not connected"},
            { GetUILocaleID("WaterPipeNotEnoughWaterCapacityNotification"), "Water facility overload"},
            { GetUILocaleID("WaterPipeNotEnoughSewageCapacityNotification"), "Sewage facility overload"},
            { GetUILocaleID("WaterPipeNotEnoughGroundwaterNotification"), "Groundwater level too low"},
            { GetUILocaleID("WaterPipeNotEnoughSurfaceWaterNotification"), "Water level too low"},
            { GetUILocaleID("WaterPipeDirtyWaterPumpNotification"), "Water pump polluted"},
            { GetUILocaleID("Building"), "BUILDING"},
            { GetUILocaleID("BuildingAbandonedCollapsedNotification"), "Collapsed"},
            { GetUILocaleID("BuildingAbandonedNotification"), "Abandoned"},
            { GetUILocaleID("BuildingCondemnedNotification"), "Condemned"},
            { GetUILocaleID("BuildingTurnedOffNotification"), "Deactivated"},
            { GetUILocaleID("BuildingHighRentNotification"), "High rent"},
            { GetUILocaleID("Traffic"), "TRAFFIC"},
            { GetUILocaleID("TrafficBottleneckNotification"), "Traffic jam"},
            { GetUILocaleID("TrafficDeadEndNotification"), "Dead end"},
            { GetUILocaleID("TrafficRoadConnectionNotification"), "Road required"},
            { GetUILocaleID("TrafficTrackConnectionNotification"), "Track not connected"},
            { GetUILocaleID("TrafficCarConnectionNotification"), "No car access"},
            { GetUILocaleID("TrafficShipConnectionNotification"), "No waterway connection"},
            { GetUILocaleID("TrafficTrainConnectionNotification"), "No track connection"},
            { GetUILocaleID("TrafficPedestrianConnectionNotification"), "No pedestrian access"},
            { GetUILocaleID("Company"), "COMPANY"},
            { GetUILocaleID("CompanyNoInputsNotification"), "High resource costs"},
            { GetUILocaleID("CompanyNoCustomersNotification"), "Not enough customers"},
            { GetUILocaleID("WorkProvider"),"WORK PROVIDER"},
            { GetUILocaleID("WorkProviderUneducatedNotification"),"Lack of Labor"},
            { GetUILocaleID("WorkProviderEducatedNotification"),"Lack of high-skill labor"},
            { GetUILocaleID("Disaster"),"DISASTER"},
            { GetUILocaleID("DisasterWeatherDamageNotification"),"Weather damage"},
            { GetUILocaleID("DisasterWeatherDestroyedNotification"),"Destroyed by weather"},
            { GetUILocaleID("DisasterWaterDamageNotification"),"Water damage"},
            { GetUILocaleID("DisasterWaterDestroyedNotification"),"Destroyed by flooding"},
            { GetUILocaleID("DisasterDestroyedNotification"),"This building has been destroyed"},
            { GetUILocaleID("Fire"),"FIRE"},
            { GetUILocaleID("FireFireNotification"),"On fire"},
            { GetUILocaleID("FireBurnedDownNotification"),"Burned down"},
            { GetUILocaleID("Garbage"),"GARBAGE"},
            { GetUILocaleID("GarbageGarbageNotification"),"Garbage piling up"},
            { GetUILocaleID("GarbageFacilityFullNotification"),"Facility full"},
            { GetUILocaleID("Healthcare"),"HEALTHCARE"},
            { GetUILocaleID("HealthcareAmbulanceNotification"),"Waiting for ambulance"},
            { GetUILocaleID("HealthcareHearseNotification"),"Waiting for a hearse"},
            { GetUILocaleID("HealthcareFacilityFullNotification"),"Facility full"},
            { GetUILocaleID("Police"),"POLICE"},
            { GetUILocaleID("PoliceTrafficAccidentNotification"),"Traffic accident"},
            { GetUILocaleID("PoliceCrimeSceneNotification"),"Crime scene"},
            { GetUILocaleID("Pollution"),"POLLUTION"},
            { GetUILocaleID("PollutionAirPollutionNotification"),"Air pollution"},
            { GetUILocaleID("PollutionNoisePollutionNotification"),"Noise pollution"},
            { GetUILocaleID("PollutionGroundPollutionNotification"),"Ground pollution"},
            { GetUILocaleID("ResourceConsumer"),"RESOURCE CONSUMER"},
            { GetUILocaleID("ResourceConsumerNoResourceNotification"),"No Emergency Shelter Supplies"},
            { GetUILocaleID("Route"),"ROUTE"},
            { GetUILocaleID("RoutePathfindNotification"),"Pathfinding failed"},
            { GetUILocaleID("TransportLine"),"TRANSPORT LINE"},
            { GetUILocaleID("TransportLineVehicleNotification"),"No vehicles"},
        });
    }
}
