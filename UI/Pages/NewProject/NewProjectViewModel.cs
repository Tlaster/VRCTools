using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.NewProject;

public class NewProjectViewModel : ViewModel
{
    private readonly SettingRepository _repository = Ioc.Default.GetRequiredService<SettingRepository>();
    public string ProjectDirectory { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public IObservable<UiUnityEditorModel?> SelectedUnityEditor => _repository.SelectedUnity;
    public void SetUnityEditorVersion(UiUnityEditorModel model) => _repository.SetSelectedUnity(model);
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityEditorList => _repository.UnityVersionList;
    public NewProjectViewModel()
    {
        _repository.ProjectDirectory.FirstOrDefaultAsync().SubscribeIn(this, x => ProjectDirectory = x ?? string.Empty);
    }

}