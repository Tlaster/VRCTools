using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.DependencyInjection;
using Realms;
using VRChatCreatorTools.Common.Extension;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.Services;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.Repository;

internal class PackageRepository
{
    private readonly Realm _realm = Ioc.Default.GetRequiredService<Realm>();
    private readonly IObservable<IReadOnlyDictionary<UiRemoteServiceModel, RemoteJsonService>> _services;

    public PackageRepository()
    {
        RemoteServices = _realm.All<DbRemoteService>().AsObservable()
            .Select(it => it.Select(UiRemoteServiceModel.FromDb).ToImmutableList());
        _services = RemoteServices.Select(it =>
            it.ToImmutableDictionary(model => model, model => new RemoteJsonService(model.Url, "")));
        InitData();
    }

    public IObservable<IReadOnlyCollection<UiRemoteServiceModel>> RemoteServices { get; }

    public async Task<IReadOnlyCollection<UiPackageModel>> FindPackage(string packageId, string? version)
    {
        var service =
            await _services.Select(it => it.Select(service => service.Value.FindPackage(packageId, version)));
        var result = await Task.WhenAll(service);
        return result.Where(it => it != null).Select(it => UiPackageModel.FromIPackageModel(it!)).ToImmutableList();
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
            AddService(name, url);
        }
    }

    public void AddService(string name, string url)
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
                CacheId = Guid.NewGuid().ToString().Replace('-', '_')
            });
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