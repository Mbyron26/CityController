using DebugConsole.Notification;
using System.Text;

namespace DebugConsole;

internal class Program {
    public static StringBuilder StringBuilderShared { get; } = new();
    public static List<Func<string>> Callback { get; } = [];

    static void Main() {
        Console.WriteLine($"CityController.DebugConsole");
        Electricity electricity = new();
        WaterPipe waterPipe = new();
        Building building = new();
        Traffic traffic = new();

        if (Callback.Count == 0) 
            return;
        Console.WriteLine($"Callback count: {Callback.Count}");
        Console.WriteLine(Environment.NewLine);
        foreach (var item in Callback) {
            Console.WriteLine(item?.Invoke());
        }
    }
}
