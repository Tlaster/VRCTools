using System.Diagnostics;
using System.Linq.Expressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.Core;
using VRChatCreatorTools.Lifecycle.Controls;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.Setting;

internal partial class SettingPage : Page<SettingViewModel>
{
    public SettingPage()
    {
        InitializeComponent();
    }

    private void ContactButton_Clicked(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        if (sender is Button { Tag: string url } && !string.IsNullOrEmpty(url))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            })?.Dispose();
        }
    }

    private void AppTheme_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        e.Handled = true;
        if (e.AddedItems.Count == 1 && e.AddedItems[0] is AppTheme item && ViewModel != null)
        {
            ViewModel.SetAppTheme(item);
        }
    }

    private async void AddUnity_OnClicked(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Title = "Please Select a Unity Editor"
        };
        var result = await dialog.ShowAsync(Window);
        if (result is { Length: 1 } && !string.IsNullOrEmpty(result[0]) && ViewModel != null)
        {
            await ViewModel.AddUnityEditor(result[0]);
        }
    }
}