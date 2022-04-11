using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Navigation;
using VRChatCreatorTools.Lifecycle.Controls;

namespace VRChatCreatorTools.UI.Pages.ProjectOverview;

internal partial class ProjectOverviewPage : Page<ProjectOverviewViewModel>
{
    public ProjectOverviewPage()
    {
        InitializeComponent();
    }

    protected override void OnCreated(object parameter)
    {
        // consider project path to be unique
        if (parameter is string path)
        {
            ViewModel.SetProjectPath(path);
        }
    }

    private void BackClicked(object? sender, RoutedEventArgs e)
    {
        GoBack();
    }
}