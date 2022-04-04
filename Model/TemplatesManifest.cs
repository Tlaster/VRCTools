using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VRChatCreatorTools.Model;


public record Template(string Path, Template.ManifestData Manifest)
{
    public record ManifestData
    (
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("defaultScene")] string DefaultScene,
        [property: JsonPropertyName("version")] string Version,
        [property: JsonPropertyName("category")] Category Category,
        [property: JsonPropertyName("dependencies")] Dictionary<string, string> Dependencies
    );
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    ProjectTemplate
}