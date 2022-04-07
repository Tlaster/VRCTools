using System.Collections.Generic;
using System.Threading.Tasks;
using Semver;
using VRChatCreatorTools.Services.Model;

namespace VRChatCreatorTools.Services;

internal interface IPackageService
{
    /// <summary>
    /// Finds the package with the given name.
    /// </summary>
    /// <param name="packageId">package id</param>
    /// <param name="version">package version, null to latest</param>
    /// <returns>Latest version of the package, return null if not found</returns>
    Task<IPackageModel?> FindPackage(string packageId, string? version = null);
    
    /// <summary>
    /// Finds the package version with the given id.
    /// </summary>
    /// <param name="packageId">package id</param>
    /// <returns>List of version that package provide, return empty list if not found</returns>
    Task<IReadOnlyCollection<string>> GetPackageVersions(string packageId);
}