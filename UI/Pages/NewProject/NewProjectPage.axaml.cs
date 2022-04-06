using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using VRChatCreatorTools.Lifecycle.Controls;
using VRChatCreatorTools.UI.Model;
using VRChatCreatorTools.UI.Pages.Setting;

namespace VRChatCreatorTools.UI.Pages.NewProject;

internal partial class NewProjectPage : Page<NewProjectViewModel>
{
    public NewProjectPage()
    {
        InitializeComponent();
    }

    private void BackClicked(object? sender, RoutedEventArgs e)
    {
        GoBack();
    }

    private async void FolderSelect_Clicked(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        var result = await dialog.ShowAsync(Window);
        if (!string.IsNullOrEmpty(result) && ViewModel != null)
        {
            ViewModel.SetProjectDirectory(result);
        }
    }
}