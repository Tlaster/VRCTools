using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using ReactiveUI;
using Realms;
using Semver;
using VRChatCreatorTools.Common.Extension;
using VRChatCreatorTools.Data.Model;
using VRChatCreatorTools.Model;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.Repository;

internal class SettingRepository
{
    private static readonly string[] SupportedUnityVersion = {
        "2019.4.31f1",
    };
    private readonly Realm _realm = Ioc.Default.GetRequiredService<Realm>();
    private readonly IObservable<DbSettingModel> _settingModel;
    public SettingRepository()
    {
        _settingModel = _realm.All<DbSettingModel>().AsObservable().Select(it => it.FirstOrDefault() ?? new DbSettingModel());
        UnityVersionList = _realm.All<DbUnityEditorModel>().AsObservable().Select(it => it.Select(x => new UiUnityEditorModel(x.Path)).ToImmutableList());
        SelectedUnity = UnityVersionList.CombineLatest(_settingModel)
        .Select(it =>
        {
            var (list, setting) = it;
            return string.IsNullOrEmpty(setting.SelectedUnityPath) ? list.FirstOrDefault() : list.FirstOrDefault(x => x.Path == setting.SelectedUnityPath);
        });
        ProjectDirectory = _settingModel.Select(it => it.ProjectDirectory);
        AppTheme = _settingModel.Select(it => Enum.Parse<AppTheme>(it.Theme));
        InitData();
    }
    public IObservable<string> ProjectDirectory { get; }
    public IObservable<AppTheme> AppTheme { get; }
    public IObservable<IReadOnlyCollection<UiUnityEditorModel>> UnityVersionList { get; }
    public IObservable<UiUnityEditorModel?> SelectedUnity { get; }

    private void InitData()
    {
        if (_realm.All<DbUnityEditorModel>().Any())
        {
            return;
        }

        _ = RefreshUnityData();
    }

    public async Task RefreshUnityData()
    {
        var items = GetFromRegistry();
        foreach (var item in items)
        {
            await AddUnityVersion(item);
        }
    }

    public async Task<bool> IsValidUnityVersion(string path)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                Arguments = "-version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            }
        };
        process.Start();
        var version = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return SupportedUnityVersion.Any(it => version.Contains(it));
    }
    
    public async Task AddUnityVersion(string path)
    {
        if (!await IsValidUnityVersion(path))
        {
            return;
        }
        _realm.Write(() =>
        {
            if (_realm.All<DbUnityEditorModel>().Any(it => it.Path == path))
            {
                return;
            }
            _realm.Add(new DbUnityEditorModel
            {
                Path = path,
            });
        });
    }
    public void RemoveUnityVersion(UiUnityEditorModel model)
    {
        _realm.Write(() =>
        {
            var item = _realm.All<DbUnityEditorModel>().FirstOrDefault(it => it.Path == model.Path);
            if (item == null)
            {
                return;
            }
            _realm.Remove(item);
        });
    }
    public void SetAppTheme(AppTheme theme)
    {
        AddOrUpdate(model => model.Theme = theme.ToString());
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