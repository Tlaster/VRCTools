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
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                    "VRChatCreatorTools");
            }
            return System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
                ".config", "VRChatCreatorTools");   
        }
    }

    public const string TemplateDownloadUrl = "https://github.com/vrchat/packages/releases/download/3.1.5/Templates-3.1.5.zip";
}