using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.DependencyInjection;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using VRChatCreatorTools.Common;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI;

namespace VRChatCreatorTools;

internal class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        EnsureWorkingDirectory();
        Ioc.Default.ConfigureServices(ConfigureServices());
    }

    private void EnsureWorkingDirectory()
    {
        if (!Directory.Exists(Consts.DocumentDirectory))
        {
            Directory.CreateDirectory(Consts.DocumentDirectory);
        }
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SettingRepository>();
        services.AddSingleton<TemplateRepository>();
        services.AddSingleton<ProjectRepository>();
        services.AddSingleton<PackageRepository>();
        services.AddSingleton<Realm>(_ =>
            Realm.GetInstance(new RealmConfiguration(Path.Combine(Consts.DocumentDirectory, ".realm"))
                { ShouldDeleteIfMigrationNeeded = true }));
        return services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime:
                classicDesktopStyleApplicationLifetime.MainWindow = new RootWindow();
                classicDesktopStyleApplicationLifetime.Exit += (_, _) =>
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