using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.DependencyInjection;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI;

namespace VRChatCreatorTools;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Ioc.Default.ConfigureServices(ConfigureServices());
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SettingRepository>();
        services.AddSingleton<Realm>(_ => Realm.GetInstance(new InMemoryConfiguration("VRChatCreatorTools")));
        return services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime:
                classicDesktopStyleApplicationLifetime.MainWindow = new CoreWindow
                {
                    Content = new RootShell(),
                    Title = "VRChat Creator Tools",
                    TransparencyLevelHint = WindowTransparencyLevel.Mica,
                    Background = null,
                    ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome,
                    ExtendClientAreaToDecorationsHint = true
                };
                break;
            case ISingleViewApplicationLifetime singleViewApplicationLifetime:
                singleViewApplicationLifetime.MainView = new RootShell();
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}