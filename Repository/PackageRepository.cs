using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.DependencyInjection;
using Realms;
using Semver;
using VRChatCreatorTools.Common;
using VRChatCreatorTools.Common.Extension;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.Services;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.Repository;

internal class PackageRepository
{
    private readonly Realm _realm = Ioc.Default.GetRequiredService<Realm>();
    private readonly IObservable<IReadOnlyDictionary<UiRemoteServiceModel, IPackageService>> _services;

    public PackageRepository()
    {
        _services = _realm.All<DbRemoteService>().AsObservable().Select(services =>
            services.OrderBy(service => service.CreatedAt).ToImmutableDictionary(UiRemoteServiceModel.FromDb,
                model => new RemoteJsonService(model.RemoteUrl) as IPackageService));
        RemoteServices = _services.Select(it => it.Keys.ToImmutableList());
        InitData();
    }

    public IObservable<IReadOnlyCollection<UiRemoteServiceModel>> RemoteServices { get; }

    public async Task<IReadOnlyCollection<UiPackageModel>> GetPackages()
    {
        var items = await _services.FirstOrDefaultAsync();
        if (items == null)
        {
            return new List<UiPackageModel>();
        }

        var packages =
            await Task.WhenAll(items.Select(async it => KeyValuePair.Create(it.Key, await it.Value.GetPackages())));
        return packages.SelectMany(pair => pair.Value.Select(it => KeyValuePair.Create(it, pair.Key)))
            .GroupBy(it => it.Key)
            .Select(it => UiPackageModel.FromIPackageModel(it.Key, it.Select(pair => pair.Value))).ToImmutableList();
    }

    public async Task<UiPackageModel?> GetPackage(string packageId,
        UiRemoteServiceModel? provider = null,
        SemVersion? version = null)
    {
        var items = await _services.FirstOrDefaultAsync();
        if (items == null)
        {
            return null;
        }

        var package = await Task.WhenAll(items.Select(async it =>
            KeyValuePair.Create(it.Key, await it.Value.GetPackage(packageId, version))));
        var dic = package.Where(it => it.Value != null).ToDictionary(x => x.Key, x => x.Value!);
        if (!dic.Any())
        {
            return null;
        }
        if (provider != null)
        {
            return dic.TryGetValue(provider, out var pkg) ? UiPackageModel.FromIPackageModel(pkg, dic.Keys) : null;
        }

        return UiPackageModel.FromIPackageModel(dic.FirstOrDefault().Value, dic.Keys);
    }

    public async Task<IReadOnlyCollection<SemVersion>> GetPackageVersions(UiRemoteServiceModel remote, string packageId)
    {
        var service = await _services.FirstOrDefaultAsync();
        if (service == null)
        {
            return new List<SemVersion>();
        }

        return await service[remote].GetPackageVersions(packageId);
    }

    private void InitData()
    {
        if (_realm.All<DbRemoteService>().Any())
        {
            return;
        }

        var loader = AvaloniaLocator.Current.GetService<IAssetLoader>();
        if (loader == null)
        {
            return;
        }

        const string path = "avares://VRChatCreatorTools/Assets/services.json";
        using var stream = loader.Open(new Uri(path));
        var data = JsonSerializer.Deserialize<List<RemoteServiceConfigModel>>(stream);
        if (data == null)
        {
            return;
        }

        foreach (var (name, url) in data)
        {
            AddService(name, url, isReadonly: true);
        }
    }

    public void AddService(string name, string url, bool isReadonly = false)
    {
        if (_realm.All<DbRemoteService>().Any(it => it.RemoteUrl == url))
        {
            return;
        }

        _realm.Write(() =>
        {
            _realm.Add(new DbRemoteService
            {
                Name = name,
                RemoteUrl = url,
                IsReadonly = isReadonly
            });
        });
    }

    public void UpdateService(UiRemoteServiceModel item, string name, string url)
    {
        var db = _realm.Find<DbRemoteService>(item.Id);
        if (db == null)
        {
            return;
        }

        _realm.Write(() =>
        {
            db.Name = name;
            db.RemoteUrl = url;
            db.UpdatedAt = DateTimeOffset.UtcNow;
        });
    }

    public void RemoveService(UiRemoteServiceModel item)
    {
        var service = _realm.All<DbRemoteService>().FirstOrDefault(it => it.RemoteUrl == item.Url);
        if (service == null)
        {
            return;
        }

        _realm.Write(() => { _realm.Remove(service); });
    }
}