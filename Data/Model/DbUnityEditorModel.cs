using Realms;

namespace VRChatCreatorTools.Data.Model;

public class DbUnityEditorModel : RealmObject
{
    [PrimaryKey] public string Path { get; set; } = string.Empty; 
    public bool IsSelected { get; set; } = false;
}