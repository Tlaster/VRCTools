using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.NewProject;

[ObservableObject]
public partial class NewProjectViewModel : ViewModel
{
    private readonly SettingRepository _repository = Ioc.Default.GetRequiredService<SettingRepository>();
    [ObservableProperty]
    private string _projectDirectory = string.Empty;
    [ObservableProperty]
    private string _projectName = string.Empty;
    public IObservable<UiUnityEditorModel?> SelectedUnityEditor => _repository.SelectedUnity;
    public void SetUnityEditorVersion(UiUnityEditorModel model) => _repository.SetSelectedUnity(model);
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityEditorList => _repository.UnityVersionList;

    internal void SetProjectDirectory(string result)
    {
        ProjectDirectory = result;
    }

    public NewProjectViewModel()
    {
        _repository.ProjectDirectory.FirstOrDefaultAsync().SubscribeIn(this, x => ProjectDirectory = x ?? string.Empty);
    }

}