using System;
using Semver;

namespace VRChatCreatorTools.UI.Model;

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