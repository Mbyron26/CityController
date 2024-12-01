using DebugConsole.Extension;
using System.Reflection;
using System.Text;
using Unity.Entities;

namespace DebugConsole.Notification;

public abstract class NotificationBase<T> where T : IQueryTypeParameter {
    public virtual string Name { get; set; }
    public string LocalizedName { get; set; }
    public string NameWithCamelCase => ConvertToLowerFiled(Name);
    public string TypeName { get; }
    public virtual List<string> NotificationRawName { get; set; } = [];
    public int EnumCount => NotificationRawName.Count;
    public static StringBuilder StringBuilderCore { get; } = new();

    protected NotificationBase() {
        Name = GetType().Name;
        TypeName = typeof(T).Name;
        LocalizedName = Name.ToUpper();
        GetNotificationRawName();
    }

    public virtual string GenerateUI() {
        StringBuilderCore.AppendLine("--- GenerateUI ---");
        StringBuilderCore.AppendLine(_ => GetSettingNames().ForEach(v => _.AppendLine($"export const {v}Binding$ = bindValue<boolean>(mod.id, \"{v}\");")));
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine(_ => GetSettingNames().ForEach(v => _.AppendLine($"export const On{v}BindingToggle = (enable: boolean) => trigger(mod.id, \"{v}\", enable);")));
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine(_ => GetSettingNames().ForEach(v => _.Append($"{v}Binding$, ")));
        StringBuilderCore.AppendLine(_ => GetSettingNames().ForEach(v => _.Append($"On{v}BindingToggle, ")));
        StringBuilderCore.AppendLine(Environment.NewLine);
        StringBuilderCore.AppendLine(_ => GetSettingNames().ForEach(v => _.AppendLine($"const {ConvertToLowerFiled(v)}Binding = useValue({v}Binding$);")));
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"<Divider></Divider>");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"<InfoPanel title={{localize(\"{Name}\")}}>");
        StringBuilderCore.AppendLine(_ => GetSettingNamesAndSvgSourcesDictionary().ForEach((k, v) => _.AppendLine($"<InfoCheckbox\r\n                        image=\"{v}\"\r\n                        label={{localize(\"{k}\")}}\r\n                        isChecked={{{ConvertToLowerFiled(k)}Binding}}\r\n                        onToggle={{(value) => On{k}BindingToggle(value)}}\r\n                        style={{{{marginBottom: \"5rem\" }}}}\r\n                    ></InfoCheckbox>")));
        StringBuilderCore.AppendLine($"</InfoPanel>");
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    public virtual string GenerateUISystem() {
        StringBuilderCore.AppendLine("--- GenerateUISystem ---");
        StringBuilderCore.AppendLine(_ => GetSettingNames().ForEach(v => _.AppendLine($"private BoolBinding {ConvertToLowerFiled(v)}Binding;")));
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"{ConvertToLowerFiled(v)}Binding = AddBoolBindingAndTriggerBinding(nameof(Setting.Instance.Notification.{v}), Setting.Instance.Notification.{v}, On{v}Toggle);")), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"#region On{Name}NotificationToggle");
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"private void On{v}Toggle(bool value) {{\r\n        {ConvertToLowerFiled(v)}Binding.Update(value);\r\n        Setting.Instance.Notification.{v} = value;\r\n        notificationControllerSystem.Enable{Name}Notification({Name}NotificationIcon.{k}, value, true);\r\n    }}")), false);
        StringBuilderCore.AppendLine($"#endregion");
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    protected string ConvertToLowerFiled(string name) => char.ToLower(name[0]) + name[1..];

    public virtual string GenerateLocale() {
        StringBuilderCore.AppendLine("--- GenerateLocale ---");
        StringBuilderCore.AppendLine($"{{ GetUILocaleID(\"{Name}\"),\"{LocalizedName}\"}},");
        StringBuilderCore.AppendLine(x => GetEnumAndLocalizedDictionary().ForEach((k, v) => x.AppendLine($"{{ GetUILocaleID(\"{Name}{k}\"),\"{v}\"}},")));
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    public Dictionary<string, string> GetEnumAndLocalizedDictionary() => Utils.CombineListsToDictionary(GetEnumList().Select(v => $"{v}").ToList(), [.. GetLocalizedId()]);

    public virtual string GenerateNotificationSystemDebugMembers() {
        StringBuilderCore.AppendLine("--- GenerateSystemDebugMethods ---");
        StringBuilderCore.AppendLine($"private EntityQuery {NameWithCamelCase}NotificationParameterQuery;");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"{NameWithCamelCase}NotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<{typeof(T).Name}>());");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"RequireForUpdate({NameWithCamelCase}NotificationParameterQuery);");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"private List<NotificationIconPrefab> Get{Name}NotificationPrefab() {{");
        StringBuilderCore.AppendLine($"List<NotificationIconPrefab> result = new();");
        StringBuilderCore.AppendLine($"var singleton = {NameWithCamelCase}NotificationParameterQuery.GetSingleton<{typeof(T).Name}>();");
        StringBuilderCore.AppendLine(x => NotificationRawName.ForEach(y => x.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{y}));")), false);
        StringBuilderCore.AppendLine($"return result;\r\n}}");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"private List<string> Get{Name}NotificationSvg() => Get{Name}NotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"private string Log{Name}NotificationSvgSources() => logBuilder.ClearAndAppendLine(LogFlag(\"Log{Name}NotificationSvgSources\")).ToString(_ => Get{Name}NotificationSvg().ForEach(v => _.AppendLine($\"\\\"{{v}}\\\",\")), false);");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"private List<string> Get{Name}NotificationPrefabName() {{");
        StringBuilderCore.AppendLine($"List<string> result = new();");
        StringBuilderCore.AppendLine($"var singleton = {NameWithCamelCase}NotificationParameterQuery.GetSingleton<{typeof(T)}>();");
        StringBuilderCore.AppendLine(x => NotificationRawName.ForEach(y => x.AppendLine($"result.Add(prefabSystem.GetPrefab<NotificationIconPrefab>(singleton.{y}).name);")), false);
        StringBuilderCore.AppendLine($"return result;\r\n}}");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"private string Log{Name}NotificationPrefabName() => logBuilder.ClearAndAppendLine(LogFlag(\"Log{Name}NotificationPrefabName\")).ToString(_ => Get{Name}NotificationPrefabName().ForEach(v => _.AppendLine($\"\\\"{{v}}\\\",\")), false);");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"Log{Name}NotificationSvgSources,");
        StringBuilderCore.AppendLine($"Log{Name}NotificationPrefabName,");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"public void Enable{Name}Notification({Name}NotificationIcon {NameWithCamelCase}NotificationIcon, bool value, bool refresh = false) {{");
        StringBuilderCore.AppendLine($"var singleton = {NameWithCamelCase}NotificationParameterQuery.GetSingleton<{typeof(T).Name}>();");
        var i = 0;
        foreach (var x in GetEnumNameAndRawNameDictionary()) {
            StringBuilderCore.AppendLine($"{(i == 0 ? "if" : "else if")} ({NameWithCamelCase}NotificationIcon == {Name}NotificationIcon.{x.Key}) {{\r\n            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.{x.Value}, value);\r\n        }}");
            i++;
        }
        StringBuilderCore.AppendLine($"if (refresh)\nRefreshIcon(); \n}}");
        StringBuilderCore.AppendLine();
        StringBuilderCore.AppendLine($"public void Set{Name}Notifications(bool refresh = true) {{");
        StringBuilderCore.AppendLine(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"Enable{Name}Notification({Name}NotificationIcon.{k}, Setting.Instance.Notification.{v});")));
        StringBuilderCore.AppendLine($"if (refresh)\r\n            RefreshIcon();\r\n    }}");
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    public virtual string GenerateSetting() {
        StringBuilderCore.AppendLine("--- GenerateSetting ---");
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine(GetSettingString(v))), false);
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"{v} = true;")), false);
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    public virtual List<string> GetSettingNames() => GetEnumList().Select(v => $"{Name}{v}").ToList();

    protected string GetSettingString(string propertyName) => $"public bool {propertyName} {{ get; set; }}";

    public virtual string GenerateEnum() {
        StringBuilderCore.AppendLine("--- GenerateEnum ---");
        StringBuilderCore.AppendLine($"public enum {Name}NotificationIcon {{");
        StringBuilderCore.ToString(_ => GetEnumList().ForEach(v => _.AppendLine($"{v},")), false);
        StringBuilderCore.AppendLine($"}}");
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    protected virtual List<string> GetEnumList() => NotificationRawName.Select(x => x[2..^6]).ToList();

    public string ShowNotificationRawName() {
        StringBuilderCore.AppendLine("--- ShowNotificationRawName ---");
        StringBuilderCore.ToString(_ => NotificationRawName.ForEach(v => _.AppendLine(v)), false);
        StringBuilderCore.AppendLine("-------------------------------------------------");
        return StringBuilderCore.ToString();
    }

    protected virtual void GetNotificationRawName() => NotificationRawName = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => f.Name.Contains("Notification") && f.FieldType == typeof(Entity)).Select(f => f.Name).ToList();

    [Obsolete("Use GenerateNotificationSystemDebugMembers instead")]
    public virtual string GenerateEnableNotification() => StringBuilderCore.ToString(_ => GetEnumNameAndRawNameDictionary().ForEach((k, v) => _.AppendLine($"if ({NameWithCamelCase}NotificationIcon == {Name}NotificationIcon.{k}) {{\r\n            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.{v}, value);\r\n        }}")));

    [Obsolete("Use GenerateNotificationSystemDebugMembers instead")]
    public virtual string GenerateSetNotifications() {
        StringBuilderCore.Clear();
        StringBuilderCore.AppendLine($"public void Set{Name}Notifications(bool refresh = true) {{");
        StringBuilderCore.ToString(_ => GetEnumNameAndSettingNameDictionary().ForEach((k, v) => _.AppendLine($"Enable{Name}Notification({Name}NotificationIcon.{k}, Setting.Instance.Notification.{v});")), false);
        StringBuilderCore.AppendLine($"if (refresh)\r\n            RefreshIcon();\r\n    }}");
        return StringBuilderCore.ToString();
    }

    public abstract string[] GetPrefabNames();
    public abstract string[] GetLocalizedId();
    public abstract string[] GetSvgSources();

    public Dictionary<string, string> GetEnumNameAndRawNameDictionary() => Utils.CombineListsToDictionary(GetEnumList(), NotificationRawName);

    protected Dictionary<string, string> GetSettingNamesAndSvgSourcesDictionary() => Utils.CombineListsToDictionary(GetSettingNames(), [.. GetSvgSources()]);

    protected Dictionary<string, string> GetEnumNameAndSettingNameDictionary() => Utils.CombineListsToDictionary(GetEnumList(), GetSettingNames());
}
