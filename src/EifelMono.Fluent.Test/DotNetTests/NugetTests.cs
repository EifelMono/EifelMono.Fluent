﻿using EifelMono.Fluent.IO;
using EifelMono.Fluent.NuGet;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.DotNetTests
{
#pragma warning disable IDE1006 // Naming Styles
    public class NugetTests : XunitCore
    {
        public NugetTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void GetPackageVersionsTest()
        {
            {
                var (Ok, Value) = await nuget.org.GetPackageVersionsAsync("EifelMono.Fluent", false);
                Assert.True(Ok);
                Dump(Value, "Versions for EifelMono.Fluent");
                Assert.NotEmpty(Value);
            }

            {
                var (Ok, Value) = await nuget.org.GetPackageVersionsAsync("EifelMono.Fluent", true);
                Assert.False(Ok);
                Dump(Value, "Versions for EifelMono.Fluent");
                Assert.Empty(Value);
            }
        }

        [Fact]
        public async void GetPackageDownloadTest()
        {
            {
                WriteLine($"EifelMono.Fluent Download Folder {DirectoryPath.OS.Temp} ");
                var result = await nuget.org.DownloadLatestPackageAsync("EifelMono.Fluent", DirectoryPath.OS.Temp);
                Assert.True(result.Ok);
                Dump(result, "nuget.DownloadLatestPackageAsync");
            }
            {
                WriteLine($"EifelMono.Fluent Download Folder {DirectoryPath.OS.Temp}");
                var result = await nuget.org.DownloadLatestPreReleasePackageAsync("EifelMono.Fluent", DirectoryPath.OS.Temp);
                Assert.False(result.Ok);
                Dump(result, "nuget.DownloadLatestPreReleasePackageAsync");
            }
        }


    }
#pragma warning restore IDE1006 // Naming Styles
}