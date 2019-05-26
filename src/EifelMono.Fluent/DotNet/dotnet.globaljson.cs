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

            public class GlobalJsonBackwards : Backwards
            {
                public GlobalJsonBackwards() { }
                public GlobalJsonBackwards(Backwards backwards)
                {
                    FileName = backwards.FileName;
                    Exists = backwards.Exists;
                    Version = fluent.Try(() => backwards.Exists ? FileName.ReadJson<DotNet.Classes.GlobalJson>().Sdk.Version : "");
                }
                public string Version { get; set; }
                public override string ToString()
                    => $"{Version} {base.ToString()}";
            }

            public static bool CurrentExist
                => DirectoryPath.OS.Current.CloneToFilePath(FileName).Exists;

            private static async Task<List<GlobalJsonBackwards>> GetFilesBackwardsAsync(DirectoryPath directoryPath, bool existingOnly = false)
                => (await (directoryPath ?? DirectoryPath.OS.Current).GetFilesBackwardsAsync(FileName, existingOnly).ConfigureAwait(false))
                    .Select(item => new GlobalJsonBackwards(item)).ToList();

            public static Task<List<GlobalJsonBackwards>> GetFilesBackwardsAsync(DirectoryPath directoryPath = null)
                => GetFilesBackwardsAsync(directoryPath ?? DirectoryPath.OS.Current, false);
            public static Task<List<GlobalJsonBackwards>> GetExistingFilesBackwardsAsync(DirectoryPath directoryPath = null)
                => GetFilesBackwardsAsync(directoryPath ?? DirectoryPath.OS.Current, true);

            public static DotNet.Classes.GlobalJson NewWithVersion(string version)
                => new DotNet.Classes.GlobalJson(version);
        }
    }
}
#endif
