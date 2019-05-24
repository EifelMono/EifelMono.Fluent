#if ! NETSTANDARD1_6
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent.DotNet
{
    public static partial class dotnet
    {
        public static class GlobalJson
        {

            public static FilePath FileName
                => DirectoryPath.OS.Current.CloneToFilePath(Classes.GlobalJson.FileName);
            public static bool Exists
                => FileName.Exists;
            public static Classes.GlobalJson NewWithVersion(string version)
                => new Classes.GlobalJson(version);
        }
    }
}
#endif
