using System;
using System.IO;
using MongoDB.Bson;
using Realms;
using VRChatCreatorTools.Common;
using VRChatCreatorTools.Model;

namespace VRChatCreatorTools.Data.Model;

internal class DbSettingModel : RealmObject
{
    [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string SelectedUnityPath { get; set; } = string.Empty;
    public string Theme { get; set; } = AppTheme.Dark.ToString();
    public string ProjectDirectory { get; set; } = Path.Combine(Consts.DocumentDirectory, "VRChatProjects");
}