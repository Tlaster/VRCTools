using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VRChatCreatorTools.Services.Model;

namespace VRChatCreatorTools.Services;

/// About the package version:
/// From https://vrchat.github.io/packages/ , the last item is the last version of the package
/// But not sure if it is the same for any other package sources, since there's no other package source right now :)

internal class RemoteJsonService : IPackageService
{
    private readonly string _remoteJsonUrl;
    private readonly string _cacheFile;
    private RemoteJsonServiceModel? _remoteJsonServiceModel;
    

    public RemoteJsonService(string remoteJsonUrl, string cacheFile)
    {
        _remoteJsonUrl = remoteJsonUrl;
        _cacheFile = cacheFile;
    }

    public async Task<IPackageModel?> FindPackage(string packageId, string? version)
    {
        await LoadCache();
        if (_remoteJsonServiceModel == null || !_remoteJsonServiceModel.Packages.TryGetValue(packageId, out var model))
        {
            return null;
        }
        var item = version switch
        {
            // TODO: Handle versions, since the last item might not be the latest
            null => model.Versions.Values.LastOrDefault(),// since we don't know the version, and the version might not be semver, we just get the latest
            _ =>  model.Versions.Values.FirstOrDefault(it => it.Version == version),
        };
        return item == null ? null : RemoteJsonPackageModel.FromJsonModel(item);
    }

    public async Task<IReadOnlyCollection<string>> GetPackageVersions(string packageId)
    {
        await LoadCache();
        if (_remoteJsonServiceModel == null || !_remoteJsonServiceModel.Packages.TryGetValue(packageId, out var model))
        {
            return new List<string>();
        }

        return model.Versions.Values.Select(it => it.Version).ToImmutableList();
    }

    private async Task DownloadCache()
    {
        using var httpClient = new System.Net.Http.HttpClient();
        using var response = await httpClient.GetAsync(_remoteJsonUrl);
        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = new FileStream(_cacheFile, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fileStream);
    }
    
    private async Task LoadCache()
    {
        if (_remoteJsonServiceModel != null)
        {
            return;
        }
        if (!File.Exists(_cacheFile))
        {
            await DownloadCache();
        }

        await using var fileStream = new FileStream(_cacheFile, FileMode.Open, FileAccess.Read);
        await using var stream = new MemoryStream();
        await fileStream.CopyToAsync(stream);
        stream.Position = 0;
        _remoteJsonServiceModel = await JsonSerializer.DeserializeAsync<RemoteJsonServiceModel>(stream);
    }

}