using Game.Prefabs;

namespace DebugConsole.Notification;

internal class Traffic : NotificationBase<TrafficConfigurationData> {
    public Traffic() {
        //Program.OnPrinted += ShowNotificationRawName;
        //Program.OnPrinted += GenerateEnum;
        //Program.OnPrinted += GenerateSetting;
        //Program.OnPrinted += GenerateNotificationSystemDebugMembers;
        //Program.OnPrinted += GenerateLocale;
        //Program.OnPrinted += GenerateUISystem;
        //Program.OnPrinted += GenerateUI;
    }

    protected override List<string> GetEnumList() => NotificationRawName.Select(item => $"{item[2..]}").ToList();

    public override string[] GetPrefabNames() => [
        "Traffic Bottleneck Notification",
        "Dead End",
        "No Road Access",
        "Track Not Connected",
        "No Car Access",
        "No Watercraft Access",
        "No Train Access",
        "No Pedestrian Access",
    ];

    public override string[] GetLocalizedId() => [
        "Traffic jam",
        "Dead end",
        "Road required",
        "Track not connected",
        "No car access",
        "No waterway connection",
        "No track connection",
        "No pedestrian access"
    ];

    public override string[] GetSvgSources() => [
        "media/Game/Notifications/TrafficBottleneck.svg",
        "media/Game/Notifications/DeadEnd.svg",
        "media/Game/Notifications/RoadNotConnected.svg",
        "media/Game/Notifications/TrackNotConnected.svg",
        "media/Game/Notifications/NoCarAccess.svg",
        "media/Game/Notifications/NoBoatAccess.svg",
        "media/Game/Notifications/NoTrainAccess.svg",
        "media/Game/Notifications/NoPedestrianAccess.svg",
    ];
}
