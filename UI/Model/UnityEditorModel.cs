using VRChatCreatorTools.Data.Model;

namespace VRChatCreatorTools.UI.Model;

public record UiUnityEditorModel(string Path, bool IsSelected)
{
    public static UiUnityEditorModel FromDb(DbUnityEditorModel model) => new(model.Path, model.IsSelected);
};