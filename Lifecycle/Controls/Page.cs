using System;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace VRChatCreatorTools.Lifecycle.Controls;

public class Page : UserControl
{
    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}