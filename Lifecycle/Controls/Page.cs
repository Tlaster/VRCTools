using System;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace VRChatCreatorTools.Lifecycle.Controls;

public class Page : UserControl
{
    protected Window Window => this.FindAncestorOfType<Window>();
    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    protected void GoBack()
    {
        this.FindAncestorOfType<Frame>().GoBack(new SlideNavigationTransitionInfo
            { Effect = SlideNavigationTransitionEffect.FromLeft });
    }

    protected void Navigate<T>(object? parameter = null)
    {
        this.FindAncestorOfType<Frame>().Navigate(typeof(T), parameter, new SlideNavigationTransitionInfo());
    }
}