using DebugConsole.Attributes;
using DebugConsole.Extension;
using Game.Prefabs;

namespace DebugConsole.Notification;

internal class Electricity : NotificationBase {
    public Electricity() {
        NotificationRawName = Utils.GetNotificationRawName<ElectricityParameterData>();
        NotificationRawName.Add("m_LowVoltageNotConnectedPrefab");
        NotificationRawName.Add("m_HighVoltageNotConnectedPrefab");
        //Invokes.Add(GenerateNotificationRawName);
        //Invokes.Add(GenerateEnum);
        //Invokes.Add(GenerateSetting);
        //Invokes.Add(GenerateGetNotificationPrefab);
        //Invokes.Add(GenerateGetNotificationPrefabName);
        //Invokes.Add(GenerateEnableNotification);
        //Invokes.Add(GenerateSetNotifications);
        //Invokes.Add(GenerateLocale);
        //Invokes.Add(GenerateUI);
    }

    public override string GenerateUI() {
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"private BoolBinding {ConvertToLowerFiled(v)}Binding;")));
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"{ConvertToLowerFiled(v)}Binding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.{v}), Setting.Instance.Notification.{v}, On{v}Toggle);")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"private void On{v}Toggle(bool value) {{\r\n        {ConvertToLowerFiled(v)}Binding.Update(value);\r\n        Setting.Instance.Notification.{v} = value;\r\n        notificationControllerSystem.EnableElectricityNotification(ElectricityNotificationIcon.{k}, value, true);\r\n    }}")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"export const {v}Binding$ = bindValue<boolean>(mod.id, \"{v}\");")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"export const On{v}BindingToggle = (enable: boolean) => trigger(mod.id, \"{v}\", enable);")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.Append($"{v}Binding$, ")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.Append($"On{v}BindingToggle, ")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"const {ConvertToLowerFiled(v)}Binding = useValue({v}Binding$);")), false);
        StringBuilderCore.ToString(_ => GetSettingNamesAndSvgSourcesDictionary().ForEach((k, v) => _.AppendLine($"<InfoCheckbox\r\n                        image=\"{v}\"\r\n                        label={{localize(\"{k}\")}}\r\n                        isChecked={{{ConvertToLowerFiled(k)}Binding}}\r\n                        onToggle={{(value) => On{k}BindingToggle(value)}}\r\n                        style={{{{marginBottom: \"5rem\" }}}}\r\n                    ></InfoCheckbox>")), false);
        return StringBuilderCore.ToString();
    }

    [GenerateMethodName("SetElectricityNotifications")]
    public override string GenerateSetNotifications() => StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"EnableElectricityNotification(ElectricityNotificationIcon.{k}, Setting.Instance.Notification.{v});")));

    [GenerateMethodName("EnableElectricityNotification")]
    public override string GenerateEnableNotification() => StringBuilderCore.ToString(_ => GetEnumNameAndRawNameDictionary().ForEach((k, v) => _.AppendLine($"else if (electricityNotificationIcon == ElectricityNotificationIcon.{k}) {{\nEntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.{v}, value);}}")));

    [GenerateMethodName("GetElectricityNotificationPrefabName")]
    public override string GenerateGetNotificationPrefabName() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}).name);")));

    [GenerateMethodName("GetElectricityNotificationPrefab")]
    public override string GenerateGetNotificationPrefab() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}));")));

    [GenerateEnumName("ElectricityNotificationIcon")]
    public override string GenerateEnum() => StringBuilderCore.ToString(_ => GetEnumList().ForEach(v => _.AppendLine($"{v},")));

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

    public override List<string> GetSettingNames() => GetEnumList().Select(_ => $"Electricity{_}").ToList();

}
