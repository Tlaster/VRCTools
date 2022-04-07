using MongoDB.Bson;
using Realms;

namespace VRChatCreatorTools.Data.Model;

internal class DbRemoteService : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string RemoteUrl { get; set; } = string.Empty;
    [Required] public string Author { get; set; } = string.Empty;
    [Required] public string CacheId { get; set; } = string.Empty;
}