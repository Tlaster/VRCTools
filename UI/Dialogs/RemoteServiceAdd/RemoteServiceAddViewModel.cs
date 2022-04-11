using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;

namespace VRChatCreatorTools.UI.Dialogs.RemoteServiceAdd;

internal partial class RemoteServiceAddViewModel : ViewModel
{
    private readonly PackageRepository _packageRepository = Ioc.Default.GetRequiredService<PackageRepository>();
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _url = string.Empty;
    [ICommand]
    private void Save()
    {
        _packageRepository.AddService(Name, Url);
    }
}