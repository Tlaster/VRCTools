using MongoDB.Bson;
using Realms;

namespace VRChatCreatorTools.Data.Model;

public class DbSettingModel : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string SelectedUnityPath { get; set; } = string.Empty;
}