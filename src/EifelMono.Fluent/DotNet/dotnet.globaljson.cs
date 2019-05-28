using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Log;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{
    public static partial class dotnet
    {
        public static class GlobalJson
        {
            public static string FileName = "global.json";

            public class GlobalJsonBackwards : DirectoryPath.Backwards
            {
                public GlobalJsonBackwards() { }
                public GlobalJsonBackwards(DirectoryPath.Backwards backwards)
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

            public static Task<List<GlobalJsonBackwards>> GetFilesBackwardsAsync(DirectoryPath directoryPath = default)
                => GetFilesBackwardsAsync(directoryPath ?? DirectoryPath.OS.Current, false);
            public static Task<List<GlobalJsonBackwards>> GetExistingFilesBackwardsAsync(DirectoryPath directoryPath = default)
                => GetFilesBackwardsAsync(directoryPath ?? DirectoryPath.OS.Current, true);

            public static DotNet.Classes.GlobalJson NewWithVersion(string version)
                => new DotNet.Classes.GlobalJson(version);

            public static bool Create(string version, DirectoryPath directoryPath= default)
            {
                directoryPath ??= DirectoryPath.OS.Current;
                try
                {
                    var fileName = directoryPath.CloneToFilePath(FileName).EnsureDirectoryExist();
                    return fileName.WriteJsonSafe(NewWithVersion(version)).Ok;
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                return false;
            }
        }
    }
}
