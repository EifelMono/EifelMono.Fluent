using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Log;
using Flurl.Http;

namespace EifelMono.Fluent.Nuget
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class nuget
#pragma warning restore IDE1006 // Naming Styles
    {

        public class Nuget
        {
            public static readonly string s_apiSearch = "api-v2v3search-0";
            public Uri Uri { get; }
            public string ServerProtocol { get; }
            public string ServerName { get; }
            public Nuget(string uri)
            {
                Uri = new Uri(uri);
            }

            public List<string> PackageVersions { get; set; }
            public List<string> PackagePreReleaseVersions { get; set; }

            public virtual void ResetDatas()
            {
                PackageVersions = null;
                PackagePreReleaseVersions = null;
            }

            public async Task<(bool Ok, List<string> Value)> GetPackageVersionsAsync(string packageName, bool preRelease = false, CancellationToken cancelationToken = default)
            {
                try
                {
                    if (PackagePreReleaseVersions is null)
                    {
                        var autocompleted = await $"{Uri.Scheme}://{s_apiSearch}.{Uri.Host}/autocomplete?id={packageName}&PreRelease=true"
                            .GetJsonAsync<Classes.AutoComplete>(cancelationToken);
                        PackageVersions = autocompleted.Data.Where(d => !d.Contains("-")).ToList();
                        PackagePreReleaseVersions = autocompleted.Data.Where(d => d.Contains("-")).ToList();
                    }
                    if (preRelease)
                        return (PackagePreReleaseVersions.Count() > 0, PackagePreReleaseVersions);
                    else
                        return (PackageVersions.Count() > 0, PackageVersions);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                return (false, null);
            }

            public async Task<(bool Ok, FilePath FileName)> DownloadPackageAsync(string packageName, string version, DirectoryPath destination, CancellationToken cancelationToken = default)
            {
                var fileName = destination.CloneToFilePath($"{packageName}.{version}.nupkg");
                try
                {
                    await $"{Uri.Scheme}://{Uri.Host}/api/v2/package/{packageName}/{version}"
                        .DownloadFileAsync(fileName.DirectoryName, fileName.FileName, cancellationToken: cancelationToken);
                    return (true, fileName);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    return (false, fileName);
                }
            }
        }

        public class NugetPackage : Nuget
        {
            public string PackageName { get; private set; }
            public NugetPackage(string uri, string packageName) : base(uri)
            {
                PackageName = packageName;
            }

            public Task<(bool Ok, List<string> Value)> GetPackageVersionsAsync(bool preRelease = false, CancellationToken cancelationToken = default)
                => GetPackageVersionsAsync(PackageName, preRelease, cancelationToken);

            public async Task<(bool Ok, FilePath FileName)> DownloadLatestPackagAsync(DirectoryPath destination, CancellationToken cancelationToken = default)
            {
                if (await GetPackageVersionsAsync(false, cancelationToken).ConfigureAwait(false) is var lastVersion && lastVersion.Ok)
                    return await DownloadPackageAsync(PackageName, lastVersion.Value.Last(), destination, cancelationToken).ConfigureAwait(false);
                return (false, null);
            }

            public async Task<(bool Ok, FilePath FileName)> DownloadLatestPreReleasePackageAsync(DirectoryPath destination, CancellationToken cancelationToken = default)
            {
                if (await GetPackageVersionsAsync(true, cancelationToken).ConfigureAwait(false) is var lastVersion && lastVersion.Ok)
                    return await DownloadPackageAsync(PackageName, lastVersion.Value.Last(), destination, cancelationToken).ConfigureAwait(false);
                return (false, null);
            }
        }
    }
}
