using VRChatCreatorTools.Lifecycle.Controls;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Dialogs.RemoteServiceEdit;

internal partial class RemoteServiceEditDialog : Dialog<RemoteServiceEditViewModel>
{
    public RemoteServiceEditDialog()
    {
        InitializeComponent();
    }
    public RemoteServiceEditDialog(UiRemoteServiceModel item)
    {
        InitializeComponent();
        ViewModel.Load(item);
    }
}