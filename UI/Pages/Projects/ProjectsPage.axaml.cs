using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VRChatCreatorTools.Lifecycle.Controls;
using VRChatCreatorTools.UI.Pages.NewProject;

namespace VRChatCreatorTools.UI.Pages.Projects;

public partial class ProjectsPage : Page
{
    public ProjectsPage()
    {
        InitializeComponent();
    }

    private void NewProjectClicked(object? sender, RoutedEventArgs e)
    {
        Navigate<NewProjectPage>();
    }
}