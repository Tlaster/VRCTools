using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;

namespace VRChatCreatorTools.UI;

public partial class RootWindow : CoreWindow
{
    public RootWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}