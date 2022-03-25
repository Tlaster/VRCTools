using MongoDB.Bson;
using Realms;

namespace VRChatCreatorTools.Data.Model;

public class ProjectModel : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Path { get; set; } = string.Empty;
}