using VRChatCreatorTools.Data.Model;

namespace VRChatCreatorTools.UI.Model;

public record UiProjectModel(string Path, string Name)
{
    public static UiProjectModel FromDbModel(DbProjectModel model)
    {
        return new UiProjectModel(model.Path, model.Name);
    }
}
