using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Realms.Sync;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.Setting;

[ObservableObject]
internal sealed partial class SettingViewModel : ViewModel
{
    private readonly SettingRepository _repository = Ioc.Default.GetRequiredService<SettingRepository>();
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityList => _repository.UnityVersionList;
    public IObservable<UiUnityEditorModel?> SelectedUnity => _repository.SelectedUnity;
    public IEnumerable<AppTheme> AllAppTheme => Enum.GetValues<AppTheme>();
    public IObservable<AppTheme> AppTheme => _repository.AppTheme;
    public void SetAppTheme(AppTheme theme) => _repository.SetAppTheme(theme);


    [ICommand]
    private void RemoveUnityEditor(UiUnityEditorModel model)
    {
        _repository.RemoveUnityVersion(model);
    }
    
    public async Task AddUnityEditor(string path)
    {
        await _repository.AddUnityVersion(path);
    }

    [ICommand]
    private async Task RefreshUnityEditor()
    {
        await _repository.RefreshUnityData();
    }
}