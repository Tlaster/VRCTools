using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.ProjectOverview;

[ObservableObject]
internal partial class ProjectOverviewViewModel : ViewModel
{
    private readonly ProjectRepository _projectRepository = Ioc.Default.GetRequiredService<ProjectRepository>();
    private readonly BehaviorSubject<string> _projectPath = new(string.Empty);

    public ProjectOverviewViewModel()
    {
        Project = _projectPath.SelectMany(it => _projectRepository.FindByPath(it));
        ProjectMeta = _projectPath.Where(it => !string.IsNullOrEmpty(it))
            .SelectMany(it => Observable.FromAsync(async () => await _projectRepository.ParseProjectMeta(it)));
    }

    public IObservable<UiProjectModel?> Project { get; }
    public IObservable<UiProjectMetaModel> ProjectMeta { get; }
    public void SetProjectPath(string path)
    {
        _projectPath.OnNext(path);
    }
}