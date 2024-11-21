using DebugConsole.Attributes;
using DebugConsole.Extension;
using Game.Prefabs;

namespace DebugConsole.Notification;

internal class Building : NotificationBase {
    public Building() {
        NotificationRawName = Utils.GetNotificationRawName<BuildingConfigurationData>();
        //Invokes.Add(GenerateNotificationRawName);
        //Invokes.Add(GenerateNotificationEnum);
        //Invokes.Add(GenerateSetting);
        //Invokes.Add(GenerateGetBuildingNotificationPrefab);
        //Invokes.Add(GenerateGetBuildingNotificationPrefabName);
        //Invokes.Add(GenerateEnableBuildingNotification);
        //Invokes.Add(GenerateSetBuildingNotifications);
        //Invokes.Add(GenerateUI);
        //Invokes.Add(GenerateLocale);
    }

    public override string GenerateUI() {
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"private BoolBinding {ConvertToLowerFiled(v)}Binding;")));
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"{ConvertToLowerFiled(v)}Binding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.{v}), Setting.Instance.Notification.{v}, On{v}Toggle);")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"private void On{v}Toggle(bool value) {{\r\n        {ConvertToLowerFiled(v)}Binding.Update(value);\r\n        Setting.Instance.Notification.{v} = value;\r\n        notificationControllerSystem.EnableBuildingNotification(BuildingNotificationIcon.{k}, value, true);\r\n    }}")), false);
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
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNamesAndSvgSourcesDictionary().ForEach((k, v) => _.AppendLine($"<InfoCheckbox\r\n                        image=\"{v}\"\r\n                        label={{localize(\"{k}\")}}\r\n                        isChecked={{{ConvertToLowerFiled(k)}Binding}}\r\n                        onToggle={{(value) => On{k}BindingToggle(value)}}\r\n                        style={{{{marginBottom: \"5rem\" }}}}\r\n                    ></InfoCheckbox>")), false);
        return StringBuilderCore.ToString();
    }

    [GenerateMethodName("SetBuildingNotifications")]
    public override string GenerateSetNotifications() => StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"EnableBuildingNotification(BuildingNotificationIcon.{k}, Setting.Instance.Notification.{v});")));

    [GenerateMethodName("EnableBuildingNotification")]
    public override string GenerateEnableNotification() => StringBuilderCore.ToString(_ => GetEnumNameAndRawNameDictionary().ForEach((k, v) => _.AppendLine($"else if (buildingNotificationIcon == BuildingNotificationIcon.{k}) {{\r\n            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.{v}, value);\r\n        }}")));

    [GenerateMethodName("GetBuildingNotificationPrefabName")]
    public override string GenerateGetNotificationPrefabName() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}).name);")));

    [GenerateMethodName("GetBuildingNotificationPrefab")]
    public override string GenerateGetNotificationPrefab() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}));")));

    [GenerateEnumName("BuildingNotificationIcon")]
    public override string GenerateEnum() => StringBuilderCore.ToString(_ => GetEnumList().ForEach(v => _.AppendLine($"{v},")));

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

    public override List<string> GetSettingNames() => GetEnumList().Select(_ => $"Building{_}").ToList();

}
