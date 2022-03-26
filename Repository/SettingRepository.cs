using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using ReactiveUI;
using Realms;
using VRChatCreatorTools.Common.Extension;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.Repository;

public class SettingRepository
{
    private readonly Realm _realm = Ioc.Default.GetService<Realm>()!;
    private readonly IObservable<DbSettingModel> _settingModel;
    public SettingRepository()
    {
        _settingModel = _realm.All<DbSettingModel>().AsObservable().Select(it => it.FirstOrDefault() ?? new DbSettingModel());
        UnityVersionList = _realm.All<DbUnityEditorModel>().AsObservable().Zip(_settingModel)
            .Select(it =>
            {
                var (items, setting) = it;
                return items.Select(x => new UiUnityEditorModel(x.Path, setting.SelectedUnityPath == x.Path))
                        .ToImmutableList();
            });
        InitData();
    }
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityVersionList { get; }
    public IObservable<UiUnityEditorModel?> SelectedUnity =>
        UnityVersionList.Select(x => x.FirstOrDefault(y => y.IsSelected));

    private void InitData()
    {
        if (_realm.All<DbUnityEditorModel>().Any())
        {
            return;
        }

        RefreshUnityData();
    }

    public void RefreshUnityData()
    {
        var items = GetFromRegistry();
        _realm.Write(() =>
        {
            foreach (var item in items)
            {
                _realm.Add(new DbUnityEditorModel
                {
                    Path = item,
                });
            }
        });
    }

    public void SetSelectedUnity(UiUnityEditorModel item)
    {
        AddOrUpdate(model => model.SelectedUnityPath = item.Path);
    }

    private void AddOrUpdate(Action<DbSettingModel> action)
    {
        _realm.Write(() =>
        {
            if (!_realm.All<DbSettingModel>().Any())
            {
                _realm.Add(new DbSettingModel());
            }
            action(_realm.All<DbSettingModel>().First());
        });
    }

    private static IReadOnlyCollection<string> GetFromRegistry()
    {
        var items = new List<string>();
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return items;
        }

        AddIfValidAndNew(
            Registry.GetValue(
                "HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Unity",
                "DisplayIcon", "")?.ToString(), items);
        AddIfValidAndNew(
            GetStringFromCommand(Registry
                .GetValue("HKEY_CLASSES_ROOT\\Applications\\Unity.exe\\shell\\open\\command", "", "")?.ToString()),
            items);
        AddIfValidAndNew(
            GetStringFromCommand(Registry
                .GetValue("HKEY_CLASSES_ROOT\\com.unity3d.kharma\\shell\\Open\\command", "", "")?.ToString()), items);
        AddIfValidAndNew(
            GetStringFromCommand(Registry.GetValue("HKEY_CLASSES_ROOT\\Unity package file\\DefaultIcon", "", "")
                ?.ToString()), items);
        AddIfValidAndNew(
            GetStringFromCommand(Registry.GetValue("HKEY_CLASSES_ROOT\\Unity scene file\\shell\\open\\command", "", "")
                ?.ToString()), items);
        return items;
    }

    private static string GetStringFromCommand(string? command)
    {
        if (command == null)
        {
            return "";
        }

        var strArray = command.Split(new[] { '"' });
        return strArray.Length >= 2 ? strArray[1] : "";
    }

    private static void AddIfValidAndNew(string? path, ICollection<string> items)
    {
        if (string.IsNullOrWhiteSpace(path) || !path.Contains("Unity.exe") || items.Contains(path))
        {
            return;
        }

        items.Add(path);
    }
}