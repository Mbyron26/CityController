using System.Text.Json;

namespace DebugConsole.Localization;

internal static class LocalizationUtils {
    public static Dictionary<string, string>? DeserializeLocalization() => JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization", "EN.json")));
}
