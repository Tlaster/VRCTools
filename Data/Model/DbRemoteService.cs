using System;
using MongoDB.Bson;
using Realms;

namespace VRChatCreatorTools.Data.Model;

internal class DbRemoteService : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string RemoteUrl { get; set; } = string.Empty;
    public bool IsReadonly { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}