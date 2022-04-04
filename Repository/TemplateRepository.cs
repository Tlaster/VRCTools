using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading.Tasks;
using VRChatCreatorTools.Common;
using VRChatCreatorTools.Model;

namespace VRChatCreatorTools.Repository;

public class TemplateRepository
{
    private readonly string _templateDirectory = Path.Combine(Consts.DocumentDirectory, "Templates");
    private readonly BehaviorSubject<IReadOnlyCollection<Template>> _templateList = new(new List<Template>());

    public IObservable<IReadOnlyCollection<Template>> TemplateList => _templateList;
    public TemplateRepository()
    {
        _ = ScanTemplateDirectory();
    }
    public async Task DownloadTemplate()
    {
        var client = new System.Net.Http.HttpClient();
        var result = await client.GetAsync(Consts.TemplateDownloadUrl);
        var stream = await result.Content.ReadAsStreamAsync();
        var zip = new ZipArchive(stream);
        zip.ExtractToDirectory(_templateDirectory);
        _ = ScanTemplateDirectory();
    }

    public async Task ScanTemplateDirectory()
    {
        if (!Directory.Exists(_templateDirectory))
        {
            Directory.CreateDirectory(_templateDirectory);
        }
        var directories = Directory.GetDirectories(_templateDirectory);
        const string packageFileName = "package.json";
        var tasks = directories.Select(async directory => {
            var packageFile = Path.Combine(directory, packageFileName);
            if (!File.Exists(packageFile))
            {
                return null;
            }
            var json = await File.ReadAllTextAsync(packageFile);
            var data = JsonSerializer.Deserialize<Template.ManifestData>(json);
            return data == null ? null : new Template(directory, data);
        });
        var result = await Task.WhenAll(tasks);
        _templateList.OnNext(result.Where(it => it != null).Select(it => it!).ToImmutableList());
    }
}