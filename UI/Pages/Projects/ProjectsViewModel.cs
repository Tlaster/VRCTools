using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.Projects;

[ObservableObject]
public partial class ProjectsViewModel : ViewModel
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

    public ProjectsViewModel()
    {
        _projectRepository.Projects.Subscribe(projects =>
        {
            ProjectsList = projects;
        });
    }
}