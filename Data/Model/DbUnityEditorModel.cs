using System;
using Realms;

namespace VRChatCreatorTools.Data.Model;

internal class DbUnityEditorModel : RealmObject
{
    [PrimaryKey] public string Path { get; set; } = string.Empty; 
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}