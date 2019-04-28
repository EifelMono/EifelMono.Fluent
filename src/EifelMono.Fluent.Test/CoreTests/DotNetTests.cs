using System;
using EifelMono.Fluent.DotNet;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.CoreTests
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
        public void dotnet_Infos()
        {
            WriteLine($"dotnet");

            WriteLine($"  SdkName");
            WriteLine($"      {dotnet.SdkName}");
            WriteLine($"  SdkReleaseName");
            WriteLine($"      {dotnet.SdkReleaseName}");
            WriteLine($"  SdkBetaName");
            WriteLine($"      {dotnet.SdkBetaName}");

            WriteLine($"  SdksMajorReleaseVersion");
            WriteLine($"      {dotnet.MajorReleaseVersion}");
            WriteLine($"  Sdks MajorReleaseVersion");
            WriteLine($"      {dotnet.MajorBetaVersion}");
            WriteLine($"  Sdks");
            dotnet.Sdks.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  SdksNames");
            dotnet.SdkNames.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  SdkReleaseNames");
            dotnet.SdkReleaseNames.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  SdkBetaNames");
            dotnet.SdkBetaNames.ForEach(item => WriteLine($"    {item}"));
            WriteLine($"  Runtimes");
            dotnet.Runtimes.ForEach(item => WriteLine($"    {item}"));
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
