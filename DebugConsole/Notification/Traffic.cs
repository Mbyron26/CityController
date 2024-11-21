using DebugConsole.Attributes;
using DebugConsole.Extension;
using Game.Prefabs;

namespace DebugConsole.Notification;

internal class Traffic : NotificationBase {
    public Traffic() {
        NotificationRawName = Utils.GetNotificationRawName<TrafficConfigurationData>();
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
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"private void On{v}Toggle(bool value) {{\r\n        {ConvertToLowerFiled(v)}Binding.Update(value);\r\n        Setting.Instance.Notification.{v} = value;\r\n        notificationControllerSystem.EnableTrafficNotification(TrafficNotificationIcon.{k}, value, true);\r\n    }}")), false);
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

    [GenerateMethodName("SetTrafficNotifications")]
    public override  string GenerateSetNotifications() => StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"EnableTrafficNotification(TrafficNotificationIcon.{k}, Setting.Instance.Notification.{v});")));

    [GenerateMethodName("EnableTrafficNotification")]
    public override string GenerateEnableNotification() => StringBuilderCore.ToString(_ => GetEnumNameAndRawNameDictionary().ForEach((k, v) => _.AppendLine($"else if (trafficNotificationIcon == TrafficNotificationIcon.{k}) {{\r\n            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.{v}, value);\r\n        }}")));

    [GenerateMethodName("GetTrafficNotificationPrefabName")]
    public override string GenerateGetNotificationPrefabName() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}).name);")));

    [GenerateMethodName("GetTrafficNotificationPrefab")]
    public override string GenerateGetNotificationPrefab() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}));")));

    [GenerateEnumName("TrafficNotificationIcon")]
    public override string GenerateEnum() => StringBuilderCore.ToString(_ => GetEnumList().ForEach(v => _.AppendLine($"{v},")));

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

    public override List<string> GetSettingNames() => GetEnumList().Select(_ => $"Traffic{_}").ToList();

}
