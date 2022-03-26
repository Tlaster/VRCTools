using System;
using Realms;

namespace VRChatCreatorTools.Data.Model;

public class DbUnityEditorModel : RealmObject
{
    [PrimaryKey] public string Path { get; set; } = string.Empty; 
}