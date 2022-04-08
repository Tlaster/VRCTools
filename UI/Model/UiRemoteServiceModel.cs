using VRChatCreatorTools.Data.Model;

namespace VRChatCreatorTools.UI.Model;

internal record UiRemoteServiceModel(string Name, string Url)
{
    public static UiRemoteServiceModel FromDb(DbRemoteService item) => new UiRemoteServiceModel(item.Name, item.RemoteUrl);
}
