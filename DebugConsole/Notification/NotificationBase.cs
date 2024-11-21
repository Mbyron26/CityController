using DebugConsole.Extension;
using System.Text;

namespace DebugConsole.Notification;

internal abstract class NotificationBase {
    public virtual List<string> NotificationRawName { get; set; } = [];
    protected StringBuilder StringBuilderCore => Program.StringBuilderShared;
    protected List<Func<string>> Invokes => Program.Callback;

    protected List<string> GetNotificationRawName() => NotificationRawName;
    protected string GenerateNotificationRawName() => StringBuilderCore.ToString(_ => NotificationRawName.ForEach(v => _.AppendLine(v)));
    protected string GetSettingString(string propertyName) => $"public bool {propertyName} {{ get; set; }}";

    protected abstract List<string> GetEnumList();

    public abstract string GenerateEnum();
    public abstract string GenerateGetNotificationPrefab();
    public abstract string GenerateGetNotificationPrefabName();
    public abstract string GenerateEnableNotification();
    public abstract string GenerateSetNotifications();
    public abstract string[] GetPrefabNames();
    public abstract string[] GetLocalizedId();
    public abstract string[] GetSvgSources();
    public abstract string GenerateUI();

    public virtual string GenerateSetting() {
        StringBuilderCore.Clear();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine(GetSettingString(v))));
        StringBuilderCore.AppendLine();
        StringBuilderCore.ToString(_ => GetSettingNames().ForEach(v => _.AppendLine($"{v} = true;")), false);
        return StringBuilderCore.ToString();
    }

    public Dictionary<string, string> GetEnumNameAndRawNameDictionary() => Utils.CombineListsToDictionary(GetEnumList(), GetNotificationRawName());

    protected Dictionary<string, string> GetEnumNameAndSvgSourcesDictionary() => Utils.CombineListsToDictionary(GetEnumList(), [.. GetSvgSources()]);

    protected Dictionary<string, string> GetSettingNamesAndSvgSourcesDictionary() => Utils.CombineListsToDictionary(GetSettingNames(), [.. GetSvgSources()]);

    protected Dictionary<string, string> GetEnumNameAndSettingNameDictionary() => Utils.CombineListsToDictionary(GetEnumList(), GetSettingNames());

    public virtual string GenerateLocale() => StringBuilderCore.ToString(_ => Utils.CombineListsToDictionary(GetSettingNames(), [.. GetLocalizedId()]).ForEach((k, v) => _.AppendLine($"{{ GetUILocaleID(\"{k}\"), \"{v}\"}},")));

    protected string ConvertToLowerFiled(string name) => char.ToLower(name[0]) + name[1..];

    public virtual List<string> GetSettingNames() => GetEnumList().Select(_ => _).ToList();
}
