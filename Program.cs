using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace VRChatCreatorTools;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI()
            .With(new Win32PlatformOptions
            {
                UseWindowsUIComposition = true
            });
    }
}