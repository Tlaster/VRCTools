using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VRChatCreatorTools.Lifecycle.Controls;

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
}