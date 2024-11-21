using DebugConsole.Attributes;
using DebugConsole.Extension;
using Game.Prefabs;
using Unity.Entities;

namespace DebugConsole.Notification;

internal class WaterPipe : NotificationBase {
    public WaterPipe() {
        NotificationRawName = Utils.GetNotificationRawName<WaterPipeParameterData>();
        //Invokes.Add(GenerateNotificationRawName);
        //Invokes.Add(GenerateNotificationEnum);
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
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"private void On{v}Toggle(bool value) {{\r\n        {ConvertToLowerFiled(v)}Binding.Update(value);\r\n        Setting.Instance.Notification.{v} = value;\r\n        notificationControllerSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.{k}, value, true);\r\n    }}")), false);
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


    [GenerateMethodName("SetWaterPipeNotifications")]
    public override string GenerateSetNotifications() => StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"EnableWaterPipeNotification(WaterPipeNotificationIcon.{k}, Setting.Instance.Notification.{v});")));

    [GenerateMethodName("EnableWaterPipeNotification")]
    public override string GenerateEnableNotification() => StringBuilderCore.ToString(_ => GetEnumNameAndRawNameDictionary().ForEach((k, v) => _.AppendLine($"else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.{k}) {{\n            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.{v}, value); }}")));

    [GenerateMethodName("GetWaterPipeNotificationPrefabName")]
    public override string GenerateGetNotificationPrefabName() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}).name);")));

    [GenerateMethodName("GetWaterPipeNotificationPrefab")]
    public override string GenerateGetNotificationPrefab() => StringBuilderCore.ToString(_ => GetNotificationRawName().ForEach(v => _.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{v}));")));

    [GenerateEnumName("WaterPipeNotificationIcon")]
    public override string GenerateEnum() => StringBuilderCore.ToString(_ => GetEnumList().ForEach(v => _.AppendLine($"{v},")));

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

    public override List<string> GetSettingNames() => GetEnumList().Select(_ => $"WaterPipe{_}").ToList();
}
