using System.Text.Json.Serialization;

namespace VRChatCreatorTools.Model;

internal record RemoteServiceConfigModel(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url
);