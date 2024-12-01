using DebugConsole.Attributes;
using DebugConsole.Extension;
using Game.Prefabs;

namespace DebugConsole.Notification;

internal class Building : NotificationBase<BuildingConfigurationData> {
    public Building() {
        //Program.OnPrinted += ShowNotificationRawName;
        //Program.OnPrinted += GenerateEnum;
        //Program.OnPrinted += GenerateSetting;
        //Program.OnPrinted += GenerateNotificationSystemDebugMembers;
        //Program.OnPrinted += GenerateLocale;
        //Program.OnPrinted += GenerateUISystem;
        //Program.OnPrinted += GenerateUI;
    }

    protected override List<string> GetEnumList() => NotificationRawName.Select(_ => _[2..]).ToList();

    public override string[] GetSvgSources() => [
        "media/Game/Notifications/BuildingCollapsed.svg",
        "media/Game/Notifications/BuildingAbandoned.svg",
        "media/Game/Notifications/BuildingCondemned.svg",
        "",
        "media/Game/Notifications/TurnedOff.svg",
        "media/Game/Notifications/RentTooHigh.svg",
    ];

    public override string[] GetPrefabNames() => [
        "Abandoned Collapsed",
        "Abandoned",
        "Condemned",
        "Building Level Up",
        "Turned Off",
        "Rent Too High",
    ];

    public override string[] GetLocalizedId() => [
        "Collapsed",
        "Abandoned",
        "Condemned",
        "Building Level Up",
        "Deactivated",
        "High rent"
    ];
}
