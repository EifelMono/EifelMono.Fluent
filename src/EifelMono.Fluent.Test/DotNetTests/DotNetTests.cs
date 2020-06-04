using System;
using System.Linq;
using EifelMono.Fluent.DotNet;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.DotNetTests
{
#pragma warning disable IDE1006 // Naming Styles
    public class DotNetTests : XunitCore
    {
        public DotNetTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void dotnet_OS()
        {
            WriteLine($"dotnet.OS");
            WriteLine($"  dotdotnet {dotnet.OS.dotdotnet}");
            WriteLine($"  dotnuget {dotnet.OS.dotnuget}");
            WriteLine($"  tools {dotnet.OS.tools}");
            WriteLine($"  dotnet {dotnet.OS.dotnet}");
            WriteLine($"  sdks {dotnet.OS.sdks}");
            WriteLine($"  runtimes {dotnet.OS.runtimes}");
        }

        [Fact]
        public void dotnet_ScanInfos()
        {
            WriteLine($"dotnet");

            WriteLine($"  SdkName");
            WriteLine($"      {dotnet.Scan.SdkName}");
            WriteLine($"  SdkReleaseName");
            WriteLine($"      {dotnet.Scan.SdkReleaseName}");
            WriteLine($"  SdkBetaName");
            WriteLine($"      {dotnet.Scan.SdkBetaName}");

            WriteLine($"  SdksMajorReleaseVersion");
            WriteLine($"      {dotnet.Scan.MajorReleaseVersion}");
            WriteLine($"  Sdks MajorReleaseVersion");
            WriteLine($"      {dotnet.Scan.MajorBetaVersion}");
            WriteLine($"  Sdks");
            dotnet.Scan.Sdks.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  SdksNames");
            dotnet.Scan.SdkNames.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  SdkReleaseNames");
            dotnet.Scan.SdkReleaseNames.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  SdkBetaNames");
            dotnet.Scan.SdkBetaNames.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  Runtimes");
            dotnet.Scan.Runtimes.ForEach(item => WriteLine($"    {item}"));
        }

        [Fact(Skip = "Does not work at this time")]
        public async void dotnet_ShellInfos()
        {
            WriteLine($"dotnet");
            {
                WriteLine($"  Version");
                var (Ok, Value) = await dotnet.Shell.VersionAsync();
                Assert.True(Ok);
                Assert.True(!Value.IsNullOrEmpty());
                WriteLine($"      {Value}");
            }
            {
                WriteLine($"  Sdks");
                var (Ok, Value) = await dotnet.Shell.SdksAsync();
                Assert.True(Ok);
                Assert.True(Value.Count > 0);
                foreach (var item in Value)
                {
                    Assert.True(!item.Version.IsNullOrEmpty());
                    Assert.True(!item.Directroy.IsNullOrEmpty());
                    WriteLine($"    {item.IsBeta} {item.Version} [{item.Directroy}]");
                }
            }
            {
                WriteLine($"  Runtimes");
                var (Ok, Value) = await dotnet.Shell.RuntimesAsync();
                Assert.True(Ok);
                Assert.True(Value.Count > 0);
                foreach (var item in Value)
                {
                    Assert.True(!item.Version.IsNullOrEmpty());
                    Assert.True(!item.Directroy.IsNullOrEmpty());
                    WriteLine($"    {item.IsBeta} {item.Version} [{item.Directroy}]");
                }
            }

            {
                WriteLine($"  Tools");
                var (Ok, Value) = await dotnet.Shell.ToolsAsync();
                Assert.True(Ok);
                Assert.True(Value.Count > 0);
                foreach (var item in Value)
                {
                    Assert.True(!item.Version.IsNullOrEmpty());
                    Assert.True(!item.PackageId.IsNullOrEmpty());
                    Assert.True(!item.Command.IsNullOrEmpty());
                    WriteLine($"    {item.IsBeta} {item.Version} {item.PackageId} {item.Command}");
                }
            }
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
