using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.Core;
using VRChatCreatorTools.Lifecycle.Controls;
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

    private void UnityEditorList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        e.Handled = true;
        if (e.AddedItems.Count == 1 && e.AddedItems[0] is UiUnityEditorModel item && DataContext is SettingViewModel vm)
        {
            vm.SetUnityEditorVersion(item);
        }
    }
}