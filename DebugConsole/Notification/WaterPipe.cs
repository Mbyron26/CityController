using Game.Prefabs;

namespace DebugConsole.Notification;

internal class WaterPipe : NotificationBase<WaterPipeParameterData> {
    public WaterPipe() {
        LocalizedName = "WATER RPIPE";
        //Program.OnPrinted += ShowNotificationRawName;
        //Program.OnPrinted += GenerateEnum;
        //Program.OnPrinted += GenerateSetting;
        //Program.OnPrinted += GenerateNotificationSystemDebugMembers;
        //Program.OnPrinted += GenerateLocale;
        //Program.OnPrinted += GenerateUISystem;
        //Program.OnPrinted += GenerateUI;
    }

    protected override List<string> GetEnumList() => NotificationRawName.Select(item => item[2..]).ToList();

    public override string[] GetPrefabNames() => [
        "Water Notification",
        "Dirty Water",
        "Sewage Notification",
        "Pipeline Not Connected",
        "Pipeline Not Connected - Sewage",
        "Water Not Enough Production Notification",
        "Sewage Not Enough Production Notification",
        "Not Enough Groundwater Notification",
        "Not Enough Surface Water Notification",
        "Dirty Water Pump Notification",
    ];

    public override string[] GetSvgSources() => [
        "media/Game/Notifications/NoRunningWater.svg",
        "media/Game/Notifications/ContaminatedWaterPumped.svg",
        "media/Game/Notifications/Sewage.svg",
        "media/Game/Notifications/WaterPipeDisconnected.svg",
        "media/Game/Notifications/SewagePipeDisconnected.svg",
        "media/Game/Notifications/WaterNotEnoughProduction.svg",
        "media/Game/Notifications/SewageNotEnoughProduction.svg",
        "media/Game/Notifications/GroundwaterLevelLow.svg",
        "media/Game/Notifications/SurfaceWaterLevelLow.svg",
        "media/Game/Notifications/DirtyWaterPump.svg"
    ];

    public override string[] GetLocalizedId() => [
        "Not enough water",
        "Water pump polluted",
        "Backed up sewer",
        "Water Pipe not connected",
        "Sewage Pipe not connected",
        "Water facility overload",
        "Sewage facility overload",
        "Groundwater level too low",
        "Water level too low",
        "Water pump polluted",
    ];
}
