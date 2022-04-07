using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Semver;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;

namespace VRChatCreatorTools.UI.Model;

internal record UiProjectModel(
    string Path,
    string Name,
    string UnityVersion
)
{
    public bool Exists => Directory.Exists(Path);

    public static UiProjectModel FromDbModel(DbProjectModel model)
    {
        return new UiProjectModel(model.Path, model.Name, model.UnityVersion);
    }
}

internal record UiProjectMetaModel(IReadOnlyCollection<IDependencyVersion> Dependencies)
{
    internal static UiProjectMetaModel FromUnityManifest(UnityManifest unityManifest)
    {
        var dependencies = unityManifest.Dependencies
            .Where(it => !it.Key.StartsWith("com.unity."))
            .Select(it => IDependencyVersion.FromString(it.Key, it.Value))
            .ToImmutableList();
        return new UiProjectMetaModel(dependencies);
    }
}

internal interface IDependencyVersion
{
    string Name { get; }

    internal record SenDependencyVersion(string Name, SemVersion Version) : IDependencyVersion;

    internal record GitDependencyVersion(string Name, string Url, string? Version) : IDependencyVersion;

    internal record LocalFileDependencyVersion(string Name, string Path) : IDependencyVersion;

    internal static IDependencyVersion FromString(string name, string value)
    {
        if (value.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) ||
            value.StartsWith("git", StringComparison.CurrentCultureIgnoreCase) ||
            value.StartsWith("ssh", StringComparison.CurrentCultureIgnoreCase))
        {
            if (!value.Contains('#'))
            {
                return new GitDependencyVersion(name, value, null);
            }

            var version = value[value.IndexOf('#')..].TrimStart('#');
            return new GitDependencyVersion(name, value, version);
        }
        
        if (value.StartsWith("file", StringComparison.CurrentCultureIgnoreCase))
        {
            return new LocalFileDependencyVersion(name, value);
        }

        if (SemVersion.TryParse(value, SemVersionStyles.Strict, out var result) && result != null)
        {
            return new SenDependencyVersion(name, result);
        }

        throw new ArgumentException($"Invalid dependency version: {value}");
    }
}