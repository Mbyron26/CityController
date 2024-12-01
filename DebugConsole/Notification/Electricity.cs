using Game.Prefabs;

namespace DebugConsole.Notification;

internal class Electricity : NotificationBase<ElectricityParameterData> {
    public Electricity() {
        NotificationRawName.Add("m_LowVoltageNotConnectedPrefab");
        NotificationRawName.Add("m_HighVoltageNotConnectedPrefab");
        //Program.OnPrinted += ShowNotificationRawName;
        //Program.OnPrinted += GenerateEnum;
        //Program.OnPrinted += GenerateSetting;
        //Program.OnPrinted += GenerateNotificationSystemDebugMembers;
        //Program.OnPrinted += GenerateLocale;
        //Program.OnPrinted += GenerateUISystem;
        //Program.OnPrinted += GenerateUI;
    }

    protected override List<string> GetEnumList() => NotificationRawName.Select(item => $"{item[2..^6]}").ToList();

    public override string[] GetPrefabNames() => [
        "Electricity Notification",
        "Electricity Bottleneck Notification",
        "Electricity Building Bottleneck Notification",
        "Electricity Not Enough Production Notification",
        "Electricity Transformer Out of Capacity",
        "Electricity Not Enough Connected Notification",
        "Battery Empty",
        "Powerline Not Connected - Low",
        "Powerline Not Connected",
    ];

    public override string[] GetSvgSources() => [
        "media/Game/Notifications/NotEnoughElectricity.svg",
        "media/Game/Notifications/ElectricityBottleneck.svg",
        "media/Game/Notifications/BadServiceElectricity.svg",
        "media/Game/Notifications/LowProductionElectricity.svg",
        "media/Game/Notifications/OutOfCapacityElectricity.svg",
        "media/Game/Notifications/NotEnoughOutputLinesConnected.svg",
        "media/Game/Notifications/BatteryEmpty.svg",
        "media/Game/Notifications/PowerlineDisconnectedLow.svg",
        "media/Game/Notifications/PowerlineDisconnected.svg",
    ];

    public override string[] GetLocalizedId() => [
        "Not enough electricity",
        "Electricity bottleneck",
        "Poor electricity flow",
        "Power station overload",
        "Transformer overload",
        "Not enough output lines connected",
        "Battery depleted",
        "Electric Cable not connected",
        "Power Line not connected",
    ];
}
