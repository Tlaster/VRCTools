using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Dialogs.RemoteServiceEdit;

[ObservableObject]
internal partial class RemoteServiceEditViewModel : ViewModel
{
    private readonly PackageRepository _packageRepository = Ioc.Default.GetRequiredService<PackageRepository>();
    private UiRemoteServiceModel? _item;

    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _url = string.Empty;
    
    public void Load(UiRemoteServiceModel item)
    {
        Name = item.Name;
        Url = item.Url;
        _item = item;
    }
    
    [ICommand]
    private void Save()
    {
        if (_item == null)
        {
            return;
        }
        _packageRepository.UpdateService(_item, Name, Url);
    }
}