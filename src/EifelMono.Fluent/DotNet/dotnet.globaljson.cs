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
        public static class GlobalJson
        {
            public static FilePath FileName 
                => DirectoryPath.OS.Current.CloneToFilePath(Classes.GlobalJson.FileName);
            public static bool Exists
                => FileName.Exists;

            public static Classes.GlobalJson FromVersion(string version)
                => new Classes.GlobalJson(version);
            public static Classes.GlobalJson FromMajorReleaseVersion()
                => new Classes.GlobalJson(dotnet.MajorReleaseVersion);
            public static Classes.GlobalJson FromMajorBetaVersion()
                => new Classes.GlobalJson(dotnet.MajorBetaVersion);
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
#endif
