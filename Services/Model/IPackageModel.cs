using System.Collections.Generic;
using Semver;

namespace VRChatCreatorTools.Services.Model;

internal interface IPackageModel
{
    string Id { get; }
    string Name { get; }
    SemVersion Version { get; }
    string Description { get; }
    string Url { get; }
    IReadOnlyDictionary<string, string> Dependencies { get; }
}
