using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;

namespace VRChatCreatorTools.Services.Model;

internal record RemoteJsonServiceModel
(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("author")] string Author,
    [property: JsonPropertyName("packages")]
    IReadOnlyDictionary<string, PackageModel> Packages
);

internal record RemoteJsonPackageModel(
    string Id,
    string Name,
    string Version,
    string Description,
    string Url,
    IReadOnlyDictionary<string, string> Dependencies
) : IPackageModel
{
    public static RemoteJsonPackageModel FromJsonModel(VersionModel item)
    {
        return new RemoteJsonPackageModel(
            item.Name,
            item.DisplayName,
            item.Version,
            item.Description,
            item.Url,
            new[] { item.Dependencies, item.GitDependencies }.SelectMany(it => it ?? new Dictionary<string, string>())
                .ToDictionary(it => it.Key, it => it.Value)
        );
    }
}

internal record PackageModel
(
    [property: JsonPropertyName("versions")]
    IReadOnlyDictionary<string, VersionModel> Versions
);

internal record VersionModel
(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("displayName")]
    string DisplayName,
    [property: JsonPropertyName("version")] 
    string Version,
    [property: JsonPropertyName("unity")] string Unity,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("gitDependencies")]
    IReadOnlyDictionary<string, string>? GitDependencies,
    [property: JsonPropertyName("samples")]
    IReadOnlyCollection<SampleModel>? Samples,
    [property: JsonPropertyName("vrchatVersion")]
    string? VRChatVersion,
    [property: JsonPropertyName("dependencies")]
    IReadOnlyDictionary<string, string>? Dependencies
);

internal record SampleModel
(
    [property: JsonPropertyName("displayName")]
    string DisplayName,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("path")] string Path
);