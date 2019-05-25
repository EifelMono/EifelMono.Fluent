#if ! NETSTANDARD1_6
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using static EifelMono.Fluent.IO.DirectoryPath;

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

            public static Task<List<Backwards>> FilesBackwardsAsync(DirectoryPath directoryPath = null)
                => (directoryPath ?? DirectoryPath.OS.Current).GetFilesBackwardsAsync(FileName);
            public static Task<List<Backwards>> ExistingFilesBackwardsAsync(DirectoryPath directoryPath = null)
                => (directoryPath ?? DirectoryPath.OS.Current).GetFilesBackwardsAsync(FileName, true);

            public static DotNet.Classes.GlobalJson NewWithVersion(string version)
                => new DotNet.Classes.GlobalJson(version);
        }
    }
}
#endif
