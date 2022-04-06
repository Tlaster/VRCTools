using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VRChatCreatorTools.Model;

internal record UnityManifest(
    [property: JsonPropertyName("dependencies")]
    Dictionary<string, string> Dependencies
);