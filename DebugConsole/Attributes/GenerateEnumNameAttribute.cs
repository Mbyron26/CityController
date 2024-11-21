namespace DebugConsole.Attributes;

[AttributeUsage(AttributeTargets.Method)]
internal class GenerateEnumNameAttribute(string value) : Attribute {
    public string EnumName { get; } = value;
}
