using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
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
    private readonly ProjectRepository _projectRepository = Ioc.Default.GetRequiredService<ProjectRepository>();
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(Projects))]
    private string _searchText = string.Empty;
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(Projects))]
    private IReadOnlyCollection<UiProjectModel> _projectsList = new List<UiProjectModel>();

    public IReadOnlyCollection<UiProjectModel> Projects
    {
        get
        {
            if (string.IsNullOrEmpty(SearchText) || string.IsNullOrWhiteSpace(SearchText))
            {
                return ProjectsList;
            }
            return ProjectsList
                .Where(it => it.Name.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)).ToImmutableList();
        }
    }
    
    [ICommand]
    private void DeleteProject(UiProjectModel project)
    {
        _projectRepository.Delete(project);
    }

    public ProjectsViewModel()
    {
        _projectRepository.Projects.Subscribe(projects =>
        {
            ProjectsList = projects;
        });
    }

    public void AddProject(string path)
    {
        var parent = Directory.GetParent(path)?.FullName;
        var projectName = new DirectoryInfo(path).Name;
        if (parent != null)
        {
            _projectRepository.Add(parent, projectName);
        }
    }
}