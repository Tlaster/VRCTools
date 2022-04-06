using System;
using MongoDB.Bson;
using Realms;

namespace VRChatCreatorTools.Data.Model;

internal class DbProjectModel : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Path { get; set; } = string.Empty;
    public DateTimeOffset LastVisit { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}