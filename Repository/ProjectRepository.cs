using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Realms;
using VRChatCreatorTools.Common.Extension;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.Repository;

internal class ProjectRepository
{
    private readonly Realm _realm = Ioc.Default.GetRequiredService<Realm>();
    public ProjectRepository()
    {
        Projects = _realm.All<DbProjectModel>().AsObservable().Select(list => list.Select(UiProjectModel.FromDbModel).ToImmutableList());
    }

    public IObservable<IReadOnlyCollection<UiProjectModel>> Projects { get; }

    public void Create(string projectDirectory, string projectName, Template selectedTemplate, UiUnityEditorModel selectedUnityEditor)
    {
        var target = Path.Combine(projectDirectory, projectName);
        Directory.CreateDirectory(target);
        Process.Start(
            new ProcessStartInfo
            {
                FileName = selectedUnityEditor.Path,
                Arguments = $"-cloneFromTemplate {selectedTemplate.Path} -createProject {target}"
            }
        )?.Dispose();
        Add(projectDirectory, projectName, selectedUnityEditor.Path);
    }
    
    public void Add(string projectDirectory, string projectName, string unityVersion)
    {
        var target = Path.Combine(projectDirectory, projectName);
        if (_realm.All<DbProjectModel>().Any(x => x.Path == target))
        {
            return;
        }
        _realm.Write(() =>
        {
            var dbProject = new DbProjectModel
            {
                Name = projectName,
                Path = target,
                UnityVersion = unityVersion,
            };
            _realm.Add(dbProject);
        });
    }

    public IObservable<UiProjectModel?> FindByPath(string path)
    {
        return _realm.All<DbProjectModel>().AsObservable().Select(list => list.FirstOrDefault(x => x.Path == path))
            .Select(it => it != null ? UiProjectModel.FromDbModel(it) : null);
    }

    public async Task<UiProjectMetaModel> ParseProjectMeta(string path)
    {
        var manifestFile = Path.Combine(path, "Packages", "manifest.json");
        var json = await File.ReadAllTextAsync(manifestFile);
        var unityManifest = JsonSerializer.Deserialize<UnityManifest>(json) ?? throw new Exception("Failed to parse manifest.json");
        return UiProjectMetaModel.FromUnityManifest(unityManifest);
    }

    public void Delete(UiProjectModel project)
    {
        _realm.Write(() =>
        {
            var dbProject = _realm.All<DbProjectModel>().FirstOrDefault(x => x.Path == project.Path);
            if (dbProject != null)
            {
                _realm.Remove(dbProject);
            }
        });
    }

    public void VisitProject(string path)
    {
        _realm.Write(() =>
        {
            var dbProject = _realm.All<DbProjectModel>().FirstOrDefault(x => x.Path == path);
            if (dbProject != null)
            {
                dbProject.LastVisited = DateTimeOffset.Now;
            }
        });
    }
}