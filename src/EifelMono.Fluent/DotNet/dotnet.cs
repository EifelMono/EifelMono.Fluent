#if ! NETSTANDARD1_6
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.DotNet
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class dotnet
    {
        public static IEnumerable<DirectoryPath> Sdks
            => OS.sdks.GetDirectories("*").Where(d => d.SplitValuesLast.StartsWithDigit()).Select(d => d.MakeAbsolute());
        public static IEnumerable<string> SdkNames
            => Sdks.Select(d => d.SplitValuesLast);

        public static IEnumerable<string> SdkReleaseNames
            => SdkNames.Where(name => !name.Contains("-"));
        public static IEnumerable<string> SdkBetaNames
            => SdkNames.Where(name => name.Contains("-"));
        public static string SdkName
            => SdkNames.Last() ?? "";
        public static string SdkReleaseName
            => SdkReleaseNames.Last() ?? "";
        public static string SdkBetaName
            => SdkBetaNames.Last() ?? "";

        public static string MajorReleaseVersion => SdkReleaseNames.Last() ?? "";

        public static string MajorBetaVersion => SdkBetaNames.Last() ?? "";

        public static IEnumerable<DirectoryPath> Runtimes
            => OS.runtimes.GetDirectories("*/*").Where(d => d.SplitValuesLast.StartsWithDigit()).Select(d => d.MakeAbsolute());

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
#pragma warning restore IDE1006 // Naming Styles
}
#endif
