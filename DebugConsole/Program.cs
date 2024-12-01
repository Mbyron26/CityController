using DebugConsole.Notification;
using System.Reflection;
using CityController;
using Game.Settings;

namespace DebugConsole;

internal class Program {
    public static event Func<string>? OnPrinted;

    public static void Main() {
        Console.WriteLine($"CityController.DebugConsole");
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<NotificationAttribute>() != null)) {
            var instance = Activator.CreateInstance(type);
            Console.WriteLine($"Instance of {type.Name} created.");
        }

        Console.WriteLine($"Invocation count: {OnPrinted?.GetInvocationList().Length ?? 0}");
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine(OnPrinted?.Invoke());
        Core.GetNotificationInfo();

        CityController.Settings.Setting setting = new(new Mod());
        Console.WriteLine(setting is null);
    }
}