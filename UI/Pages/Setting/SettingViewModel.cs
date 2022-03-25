using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.Setting;

internal sealed class SettingViewModel : ViewModel
{
    private readonly SettingRepository _repository = Ioc.Default.GetService<SettingRepository>()!;
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityList => _repository.UnityVersionList;
    public IObservable<UiUnityEditorModel?> SelectedUnity => _repository.SelectedUnity;
    public void SetUnityEditorVersion(UiUnityEditorModel model) => _repository.SetSelectedUnity(model);
}