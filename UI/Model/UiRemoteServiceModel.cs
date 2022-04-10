using System.Runtime.CompilerServices;
using MongoDB.Bson;
using VRChatCreatorTools.Data.Model;

namespace VRChatCreatorTools.UI.Model;

internal record UiRemoteServiceModel(ObjectId Id, string Name, string Url, bool IsReadonly)
{
    public static UiRemoteServiceModel FromDb(DbRemoteService item) => new(item.Id, item.Name, item.RemoteUrl, item.IsReadonly);
}
