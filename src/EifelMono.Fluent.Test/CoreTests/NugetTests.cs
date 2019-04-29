using EifelMono.Fluent.IO;
using EifelMono.Fluent.Nuget;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.CoreTests
{
#pragma warning disable IDE1006 // Naming Styles
    public class NugetTests : XunitCore
    {
        public NugetTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void GetPackageVersionsTest()
        {
            var (Ok, Value) = await nuget.GetPackageVersionsAsync(nuget.NugetOrg, "EifelMono.Fluent", true);
            Assert.True(Ok);
            Dump(Value, "Versions for EifelMono.Fluent");
            Assert.NotEmpty(Value);
        }

        [Fact]
        public async void GetPackageDownloadTest()
        {
            {
                var result = await nuget.DownloadLatestPackageAsync(nuget.NugetOrg, "EifelMono.Fluent", DirectoryPath.OS.Data);
                Assert.True(result.Ok);
                Dump(result, "nuget.DownloadLatestPackageAsync");
            }
            {
                var result = await nuget.DownloadLatestPreReleasePackageAsync(nuget.NugetOrg, "EifelMono.Fluent", DirectoryPath.OS.Data);
                Assert.False(result.Ok);
                Dump(result, "nuget.DownloadLatestPreReleasePackageAsync");
            }
        }


    }
#pragma warning restore IDE1006 // Naming Styles
}
