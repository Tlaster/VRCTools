using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.DependencyInjection;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using VRChatCreatorTools.Common;
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
        services.AddSingleton<TemplateRepository>();
        services.AddSingleton<ProjectRepository>();
        services.AddSingleton<Realm>(_ => Realm.GetInstance(new RealmConfiguration(Path.Combine(Consts.DocumentDirectory, ".realm"))));
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
                    // TransparencyLevelHint = WindowTransparencyLevel.Mica,
                    // Background = Brushes.Transparent,
                    // ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome,
                    // ExtendClientAreaToDecorationsHint = true,
                    Width = 1024,
                    Height = 576,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
                classicDesktopStyleApplicationLifetime.MainWindow.Closing += (_, _) =>
                {
                    Ioc.Default.GetRequiredService<Realm>().Dispose();
                };
                break;
            case ISingleViewApplicationLifetime singleViewApplicationLifetime:
                singleViewApplicationLifetime.MainView = new RootShell();
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}