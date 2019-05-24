#if ! NETSTANDARD1_6
using EifelMono.Fluent.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent.DotNet
{
    public static partial class dotnet
    {
        public static class OS
        {
            public static DirectoryPath dotnet
                => DirectoryPath.OS.SpezialDirectory.ProgramFiles.Clone("dotnet");
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
#endif
