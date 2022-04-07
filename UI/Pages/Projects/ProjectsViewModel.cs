using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.Projects;

[ObservableObject]
internal partial class ProjectsViewModel : ViewModel
{
    private readonly SettingRepository _settingRepository = Ioc.Default.GetRequiredService<SettingRepository>();
    private readonly ProjectRepository _projectRepository = Ioc.Default.GetRequiredService<ProjectRepository>();
    private readonly BehaviorSubject<string> _searchTextSubject = new(string.Empty);
    [ObservableProperty] private string _searchText = string.Empty;
    
    partial void OnSearchTextChanged(string value)
    {
        _searchTextSubject.OnNext(value);
    }

    public IObservable<IReadOnlyCollection<UiProjectModel>> Projects { get; }

    [ICommand]
    private void OpenFolder(UiProjectModel item)
    {
        if (!item.Exists)
        {
            return;
        }

        Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = item.Path })?.Dispose();
    }

    [ICommand]
    private void OpenUnity(UiProjectModel item)
    {
        if (!item.Exists)
        {
            return;
        }

        Process.Start(new ProcessStartInfo
                { UseShellExecute = true, FileName = item.UnityVersion, Arguments = $"-projectPath \"{item.Path}\"" })
            ?.Dispose();
    }

    [ICommand]
    private void DeleteProject(UiProjectModel project)
    {
        _projectRepository.Delete(project);
    }

    public ProjectsViewModel()
    {
        Projects = _projectRepository.Projects.CombineLatest(_searchTextSubject.Throttle(TimeSpan.FromSeconds(0.3)))
            .Select(it =>
            {
                var (projects, searchText) = it;
                return string.IsNullOrWhiteSpace(searchText)
                    ? projects
                    : projects.Where(
                            model => model.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                        .ToImmutableArray();
            });
    }

    public async Task AddProject(string path)
    {
        var parent = Directory.GetParent(path)?.FullName;
        var projectName = new DirectoryInfo(path).Name;
        var selectedUnity = await _settingRepository.SelectedUnity.FirstOrDefaultAsync();
        if (parent != null && !string.IsNullOrEmpty(selectedUnity?.Path))
        {
            _projectRepository.Add(parent, projectName, selectedUnity.Path);
        }
    }
}