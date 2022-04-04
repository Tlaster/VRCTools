using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using Realms;
using VRChatCreatorTools.Common.Extension;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.Repository;

public class ProjectRepository
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
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = selectedUnityEditor.Path,
                Arguments = $"-cloneFromTemplate {selectedTemplate.Path} -createProject {target}"
            }
        };
        process.Start();
        Add(projectDirectory, projectName);
    }
    
    public void Add(string projectDirectory, string projectName)
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
                Path = target
            };
            _realm.Add(dbProject);
        });
    }
}