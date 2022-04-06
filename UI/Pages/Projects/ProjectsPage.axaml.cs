using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VRChatCreatorTools.Lifecycle.Controls;
using VRChatCreatorTools.UI.Model;
using VRChatCreatorTools.UI.Pages.NewProject;
using VRChatCreatorTools.UI.Pages.ProjectOverview;

namespace VRChatCreatorTools.UI.Pages.Projects;

internal partial class ProjectsPage : Page<ProjectsViewModel>
{
    public ProjectsPage()
    {
        InitializeComponent();
    }

    private void NewProjectClicked(object? sender, RoutedEventArgs e)
    {
        Navigate<NewProjectPage>();
    }

    private void OnProjectClicked(UiProjectModel item)
    {
        Navigate<ProjectOverviewPage>(item.Path);
    }
    
    private void OnProjectOpenInFolderClicked(UiProjectModel item)
    {
        Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = item.Path })?.Dispose();
    }

    private async void OpenFolder_OnClicked(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        var picker = new OpenFolderDialog();
        var result = await picker.ShowAsync(Window);
        if (!string.IsNullOrEmpty(result))
        {
            ViewModel?.AddProject(result);
        }
    }
}