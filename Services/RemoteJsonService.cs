using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Semver;
using VRChatCreatorTools.Services.Model;

namespace VRChatCreatorTools.Services;

/// About the package version:
/// Both https://vrchat.github.io/packages/ and https://vrchat-community.github.io/curated-packages/ seems to use semver,
/// But not sure if this is the required format.
/// Since there if no official documentation, nor any other package source.
internal class RemoteJsonService : IPackageService
{
    private readonly string _remoteJsonUrl;
    private RemoteJsonServiceModel? _remoteJsonServiceModel;

    public RemoteJsonService(string remoteJsonUrl)
    {
        _remoteJsonUrl = remoteJsonUrl;
    }

    public async Task<IPackageModel?> FindPackage(string packageId, SemVersion? version)
    {
        await LoadCache();
        if (_remoteJsonServiceModel == null || !_remoteJsonServiceModel.Packages.TryGetValue(packageId, out var model))
        {
            return null;
        }

        var item = version switch
        {
            null => model.Versions.Values.MaxBy(it => it.Version),
            _ => model.Versions.Values.FirstOrDefault(it => it.Version == version),
        };
        return item == null ? null : RemoteJsonPackageModel.FromJsonModel(item);
    }

    public async Task<IReadOnlyCollection<SemVersion>> GetPackageVersions(string packageId)
    {
        await LoadCache();
        if (_remoteJsonServiceModel == null || !_remoteJsonServiceModel.Packages.TryGetValue(packageId, out var model))
        {
            return new List<SemVersion>();
        }

        return model.Versions.Values.Select(it => it.Version).ToImmutableList();
    }

    public async Task<IReadOnlyCollection<IPackageModel>> GetPackages()
    {
        await LoadCache();
        if (_remoteJsonServiceModel == null)
        {
            return new List<IPackageModel>();
        }

        return _remoteJsonServiceModel.Packages.Values.Select(it => it.Versions.Values.MaxBy(model => model.Version))
            .Where(it => it != null).Select(RemoteJsonPackageModel.FromJsonModel!).ToImmutableList();
    }

    private async Task LoadCache()
    {
        if (_remoteJsonServiceModel != null)
        {
            return;
        }

        using var httpClient = new System.Net.Http.HttpClient();
        using var response = await httpClient.GetAsync(_remoteJsonUrl);
        await using var stream = await response.Content.ReadAsStreamAsync();
        _remoteJsonServiceModel = await JsonSerializer.DeserializeAsync<RemoteJsonServiceModel>(stream);
    }
}