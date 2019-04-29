using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.IO;

namespace EifelMono.Fluent.Nuget
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class nuget
#pragma warning restore IDE1006 // Naming Styles
    {
        public static string NugetOrg { get; private set; } = "https://nuget.org";
        public static Task<(bool Ok, List<string> Value)> GetPackageVersionsAsync(string url, string packageName, bool preRelease = false, CancellationToken cancelationToken = default)
            => new NugetPackage(url, packageName).GetPackageVersionsAsync(preRelease, cancelationToken);
        public static Task<(bool Ok, FilePath FileName)> DownloadLatestPackageAsync(string url, string packageName, DirectoryPath destination, CancellationToken cancelationToken = default)
            => new NugetPackage(url, packageName).DownloadLatestPackagAsync(destination, cancelationToken);
        public static Task<(bool Ok, FilePath FileName)> DownloadLatestPreReleasePackageAsync(string url, string packageName, DirectoryPath destination, CancellationToken cancelationToken = default)
            => new NugetPackage(url, packageName).DownloadLatestPreReleasePackageAsync(destination, cancelationToken);
    }
}
