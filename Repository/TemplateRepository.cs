using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using VRChatCreatorTools.Common;

namespace VRChatCreatorTools.Repository;

public class TemplateRepository 
{
    private readonly string _templateDirectory = Path.Combine(Consts.DocumentDirectory, "Templates");
    public async Task DownloadTemplate()
    {
        var client = new System.Net.Http.HttpClient();
        var result = await client.GetAsync(Consts.TemplateDownloadUrl);
        var stream = await result.Content.ReadAsStreamAsync();
        var zip = new System.IO.Compression.ZipArchive(stream);
        zip.ExtractToDirectory(_templateDirectory);
    }

    public string ScanTemplateDirectory()
    {
        var files = Directory.GetFiles(_templateDirectory);
        if (files.Length == 0)
        {
            return null;
        }
        return files[0];
    }
}