using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.IO;

namespace EifelMono.Fluent.NuGet
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class nuget
#pragma warning restore IDE1006 // Naming Styles
    {
        public static Task<(bool Ok, List<string> Value)> GetPackageVersionsAsync(string url, string packageName, bool preRelease = false, CancellationToken cancelationToken = default)
            => new NuGetPackage(url, packageName).GetPackageVersionsAsync(preRelease, cancelationToken);
        public static Task<(bool Ok, FilePath FileName)> DownloadLatestPackageAsync(string url, string packageName, DirectoryPath destination, CancellationToken cancelationToken = default)
            => new NuGetPackage(url, packageName).DownloadLatestPackagAsync(destination, cancelationToken);
        public static Task<(bool Ok, FilePath FileName)> DownloadLatestPreReleasePackageAsync(string url, string packageName, DirectoryPath destination, CancellationToken cancelationToken = default)
            => new NuGetPackage(url, packageName).DownloadLatestPreReleasePackageAsync(destination, cancelationToken);

#pragma warning disable IDE1006 // Naming Styles
        public static partial class org
#pragma warning restore IDE1006 // Naming Styles
        {
            public static string ServerUrl { get; private set; } = "https://nuget.org";
            public static Task<(bool Ok, List<string> Value)> GetPackageVersionsAsync(string packageName, bool preRelease = false, CancellationToken cancelationToken = default)
                => new NuGetPackage(ServerUrl, packageName).GetPackageVersionsAsync(preRelease, cancelationToken);
            public static Task<(bool Ok, FilePath FileName)> DownloadLatestPackageAsync(string packageName, DirectoryPath destination, CancellationToken cancelationToken = default)
                => new NuGetPackage(ServerUrl, packageName).DownloadLatestPackagAsync(destination, cancelationToken);
            public static Task<(bool Ok, FilePath FileName)> DownloadLatestPreReleasePackageAsync(string packageName, DirectoryPath destination, CancellationToken cancelationToken = default)
                => new NuGetPackage(ServerUrl, packageName).DownloadLatestPreReleasePackageAsync(destination, cancelationToken);
        }
    }
}
