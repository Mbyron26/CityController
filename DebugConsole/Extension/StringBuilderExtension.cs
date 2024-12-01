using System.Text;

namespace DebugConsole.Extension;

public static class StringBuilderExtension {
    public static StringBuilder AppendLine(this StringBuilder stringBuilder, Action<StringBuilder> action, bool clearFirst = false) {
        if (clearFirst)
            stringBuilder.Clear();
        action(stringBuilder);
        return stringBuilder;
    }

    public static StringBuilder ClearAndAppendLine(this StringBuilder stringBuilder, string value, bool clear = true) {
        stringBuilder.Clear();
        stringBuilder.AppendLine(value);
        return stringBuilder;
    }

    public static string ToString(this StringBuilder stringBuilder, Action<StringBuilder> action, bool clear = true) {
        if (clear)
            stringBuilder.Clear();
        action(stringBuilder);
        return stringBuilder.ToString();
    }

    public static string ToString(this StringBuilder stringBuilder, List<Action<StringBuilder>> actions) {
        stringBuilder.Clear();
        foreach (var action in actions) {
            action(stringBuilder);
        }
        return stringBuilder.ToString();
    }
}
