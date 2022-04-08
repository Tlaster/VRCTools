using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;

namespace VRChatCreatorTools.UI.Model;

internal record UiProjectModel(
    string Path,
    string Name,
    string UnityVersion
)
{
    public bool Exists => Directory.Exists(Path);

    public static UiProjectModel FromDbModel(DbProjectModel model)
    {
        return new UiProjectModel(model.Path, model.Name, model.UnityVersion);
    }
}

internal record UiProjectMetaModel(IReadOnlyCollection<IDependencyVersion> Dependencies)
{
    internal static UiProjectMetaModel FromUnityManifest(UnityManifest unityManifest)
    {
        var dependencies = unityManifest.Dependencies
            .Where(it => !it.Key.StartsWith("com.unity."))
            .Select(it => IDependencyVersion.FromString(it.Key, it.Value))
            .ToImmutableList();
        return new UiProjectMetaModel(dependencies);
    }
}