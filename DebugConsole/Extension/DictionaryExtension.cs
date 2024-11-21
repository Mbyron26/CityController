namespace DebugConsole.Extension;

public static class DictionaryExtensions {
    public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<TKey, TValue> action) where TKey : notnull {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(action);
        foreach (var kvp in dictionary) {
            action(kvp.Key, kvp.Value);
        }
    }
}
