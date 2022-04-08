using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VRChatCreatorTools.Services.Model;

namespace VRChatCreatorTools.UI.Model;

internal record UiPackageModel(
    string Id,
    string Name,
    string Version,
    string Description,
    string Url,
    IReadOnlyCollection<IDependencyVersion> Dependency
)
{
    public static UiPackageModel FromIPackageModel(IPackageModel model)
    {
        return new UiPackageModel(
            model.Id,
            model.Name,
            model.Version,
            model.Description,
            model.Url,
            model.Dependencies.Select(it => IDependencyVersion.FromString(it.Key, it.Value)).ToImmutableList()
        );
    }
}