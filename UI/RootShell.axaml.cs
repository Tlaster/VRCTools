using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;
using CommunityToolkit.Mvvm.DependencyInjection;
using FluentAvalonia.Styling;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.Repository;

namespace VRChatCreatorTools.UI;

public partial class RootShell : UserControl
{
    private readonly SettingRepository _settingRepository = Ioc.Default.GetRequiredService<SettingRepository>();

    private readonly FluentAvaloniaTheme _fluentAvaloniaTheme =
        AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>()!;
    public RootShell()
    {
        InitializeComponent();
        _settingRepository.AppTheme.Subscribe(theme =>
        {
            _fluentAvaloniaTheme.RequestedTheme = theme switch
            {
                AppTheme.Light => FluentAvaloniaTheme.LightModeString,
                AppTheme.Dark => FluentAvaloniaTheme.DarkModeString,
                _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
            };
        });
    }
}