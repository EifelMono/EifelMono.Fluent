#if ! NETSTANDARD1_6
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{
    public static partial class dotnet
    {
        public static class GlobalJson
        {
            public static string FileName = "global.json";

            public static bool CurrentExist
                => DirectoryPath.OS.Current.CloneToFilePath(FileName).Exists;

            public static DotNet.Classes.GlobalJson NewWithVersion(string version)
                => new DotNet.Classes.GlobalJson(version);
        }
    }
}
#endif
