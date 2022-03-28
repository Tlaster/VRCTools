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

public partial class NewProjectPage : Page
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
        if (!string.IsNullOrEmpty(result) && DataContext is NewProjectViewModel viewModel)
        {
            viewModel.ProjectDirectory = result;
        }
    }
    
    private void UnityEditorList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        e.Handled = true;
        if (e.AddedItems.Count == 1 && e.AddedItems[0] is UiUnityEditorModel item && DataContext is NewProjectViewModel vm)
        {
            vm.SetUnityEditorVersion(item);
        }
    }

}