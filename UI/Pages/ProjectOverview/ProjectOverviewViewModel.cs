using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.ProjectOverview;

internal partial class ProjectOverviewViewModel : ViewModel
{
    private readonly ProjectRepository _projectRepository = Ioc.Default.GetRequiredService<ProjectRepository>();
    private readonly PackageRepository _packageRepository = Ioc.Default.GetRequiredService<PackageRepository>();
    private readonly BehaviorSubject<string> _projectPath = new(string.Empty);

    public ProjectOverviewViewModel()
    {
        Project = _projectPath.SelectMany(it => _projectRepository.FindByPath(it));
        ProjectMeta = _projectPath.Where(it => !string.IsNullOrEmpty(it))
            .SelectMany(it => Observable.FromAsync(async () => await _projectRepository.ParseProjectMeta(it)));
        AllPackages = Observable.FromAsync(async () => await _packageRepository.GetPackages());
        InstalledPackages = ProjectMeta.Select(it => it.Dependencies).Select(it =>
                it.Select(version => _packageRepository.GetPackage(version.Name)))
            .Select(Task.WhenAll)
            .Switch()
            .Select(list => list.Where(it => it != null).Select(it => it!).ToImmutableList());
    }

    public IObservable<UiProjectModel?> Project { get; }
    public IObservable<UiProjectMetaModel> ProjectMeta { get; }
    public IObservable<IReadOnlyCollection<UiPackageModel>> InstalledPackages { get; }
    public IObservable<IReadOnlyCollection<UiPackageModel>> AllPackages { get; }

    public void SetProjectPath(string path)
    {
        _projectPath.OnNext(path);
        _projectRepository.VisitProject(path);
    }

    [ICommand]
    private async void OpenFolder()
    {
        var item = await Project.FirstOrDefaultAsync();
        if (item is not { Exists: true })
        {
            return;
        }

        Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = item.Path })?.Dispose();
    }

    [ICommand]
    private async void OpenUnity()
    {
        var item = await Project.FirstOrDefaultAsync();
        if (item is not { Exists: true })
        {
            return;
        }

        Process.Start(new ProcessStartInfo
                { UseShellExecute = true, FileName = item.UnityVersion, Arguments = $"-projectPath \"{item.Path}\"" })
            ?.Dispose();
    }
}