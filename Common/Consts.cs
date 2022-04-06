using System.Runtime.InteropServices;

namespace VRChatCreatorTools.Common;


internal static class Consts
{
    public static string DocumentDirectory
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return System.IO.Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
                    "VRChatCreatorTools");
            }
            return System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                ".config", "VRChatCreatorTools");   
        }
    }

    public const string TemplateDownloadUrl = "http://temp.mmmlabs.com/Templates.zip";
}