namespace DebugConsole.Attributes;

[AttributeUsage(AttributeTargets.Method)]
internal class GenerateMethodNameAttribute(string value) : Attribute {
    public string Value { get; } = value;
}
