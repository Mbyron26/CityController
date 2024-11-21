using System.Reflection;

namespace DebugConsole.Notification;

internal static class Utils {
    public static Dictionary<string, string> CombineListsToDictionary(List<string> keys, List<string> values) {
        if (keys.Count != values.Count) {
            throw new ArgumentException("Both lists must be the same length.");
        }
        return keys.Zip(values, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
    }

    public static List<string> GetNotificationRawName<T>() {
        List<string> list = [];
        var fieldsWithNotification = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => f.Name.Contains("Notification")).Select(f => f.Name);
        foreach (var fieldName in fieldsWithNotification) {
            list.Add(fieldName);
        }
        return list;
    }
}
