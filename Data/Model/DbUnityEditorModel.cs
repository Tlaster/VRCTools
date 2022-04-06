using System;
using Realms;

namespace VRChatCreatorTools.Data.Model;

public class DbUnityEditorModel : RealmObject
{
    [PrimaryKey] public string Path { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}