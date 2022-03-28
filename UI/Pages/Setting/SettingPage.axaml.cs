using System.Diagnostics;
using System.Linq.Expressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.Core;
using VRChatCreatorTools.Lifecycle.Controls;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.Setting;

public partial class SettingPage : Page
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
            });
        }
    }

    private void AppTheme_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        e.Handled = true;
        if (e.AddedItems.Count == 1 && e.AddedItems[0] is AppTheme item && DataContext is SettingViewModel vm)
        {
            vm.SetAppTheme(item);
        }
    }

    private async void AddUnity_OnClicked(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        var dialog = new OpenFileDialog();
        dialog.AllowMultiple = false;
        dialog.Title = "Please Select a Unity Editor";
        var result = await dialog.ShowAsync(Window);
        if (result is { Length: 1 } && !string.IsNullOrEmpty(result[0]) && DataContext is SettingViewModel viewModel)
        {
            viewModel.AddUnityEditor(result[0]);
        }
    }

    private void RefreshUnity_OnClicked(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        if (DataContext is SettingViewModel viewModel)
        {
            viewModel.RefreshUnityEditor();
        }
    }
}