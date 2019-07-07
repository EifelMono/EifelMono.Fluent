using EifelMono.Fluent.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{
    public static partial class dotnet
    {
        public static class OS
        {
            public static DirectoryPath dotnet
                => fluent.OS.System.On(
                    () => DirectoryPath.OS.SpezialDirectory.ProgramFiles.Clone("dotnet"),
                    () => new DirectoryPath("/usr/local/share/dotnet"),
                    () => new DirectoryPath("/usr/local/share/dotnet")
                );
            public static DirectoryPath sdks
                => dotnet.Clone("sdk");
            public static DirectoryPath runtimes
                => dotnet.Clone("shared"); // With SubFolders
            public static DirectoryPath dotdotnet
                => DirectoryPath.OS.SpezialDirectory.UserProfile.Clone(".dotnet");
            public static DirectoryPath tools
                => dotdotnet.Clone("tools");
            public static DirectoryPath dotnuget
                => DirectoryPath.OS.SpezialDirectory.UserProfile.Clone(".nuget");
        }
    }
}
