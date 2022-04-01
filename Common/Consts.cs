namespace VRChatCreatorTools.Common;


internal static class Consts
{
    public static string DocumentDirectory => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "VRChatCreatorTools");
    public const string TemplateDownloadUrl = "http://temp.mmmlabs.com/Templates.zip";
}