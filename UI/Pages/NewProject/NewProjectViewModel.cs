using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using VRChatCreatorTools.Lifecycle.ViewModel;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.Repository;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Pages.NewProject;

[ObservableObject]
public partial class NewProjectViewModel : ViewModel
{
    private readonly SettingRepository _repository = Ioc.Default.GetRequiredService<SettingRepository>();
    private readonly TemplateRepository _templateRepository = Ioc.Default.GetRequiredService<TemplateRepository>();
    private readonly ProjectRepository _projectRepository = Ioc.Default.GetRequiredService<ProjectRepository>();
    [ObservableProperty]
    [AlsoNotifyCanExecuteFor(nameof(CreateCommand))]
    private string _projectDirectory = string.Empty;
    [ObservableProperty]
    [AlsoNotifyCanExecuteFor(nameof(CreateCommand))]
    private string _projectName = string.Empty;
    [ObservableProperty] 
    [AlsoNotifyCanExecuteFor(nameof(CreateCommand))]
    private UiUnityEditorModel? _selectedUnityEditor = null;
    [ObservableProperty]
    private bool _loading = false;
    [ObservableProperty]
    private Template? _selectedTemplate = null;
    public bool CanCreate => !string.IsNullOrWhiteSpace(ProjectName) && 
                             !string.IsNullOrWhiteSpace(ProjectDirectory) && 
                             SelectedUnityEditor != null &&
                             SelectedTemplate != null &&
                             !Directory.Exists(Path.Combine(ProjectDirectory, ProjectName));
    public IObservable<IReadOnlyCollection<Template>> TemplateList => _templateRepository.TemplateList;
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityEditorList => _repository.UnityVersionList;

    [ICommand(CanExecute = nameof(CanCreate))]
    private void Create()
    {
        if (SelectedTemplate == null || SelectedUnityEditor == null)
        {
            return;
        }

        _repository.SetSelectedUnity(SelectedUnityEditor);
        _projectRepository.Create(ProjectDirectory, ProjectName, SelectedTemplate, SelectedUnityEditor);
    }
    
    [ICommand]
    private async Task ScanTemplateDirectory() 
    {
        Loading = true;
        await _templateRepository.ScanTemplateDirectory();
        Loading = false;
    }

    [ICommand]
    private async Task DownloadTemplate() 
    {
        Loading = true;
        await _templateRepository.DownloadTemplate();
        Loading = false;
    }

    internal void SetProjectDirectory(string result)
    {
        ProjectDirectory = result;
    }

    public NewProjectViewModel()
    {
        _repository.ProjectDirectory.FirstOrDefaultAsync().SubscribeIn(this, x => ProjectDirectory = x ?? string.Empty);
        _repository.SelectedUnity.FirstOrDefaultAsync().SubscribeIn(this, x => SelectedUnityEditor = x);

    }

}