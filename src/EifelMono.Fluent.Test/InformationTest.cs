using System;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class InformationTest : XunitCore
    {
        public InformationTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void FilePath_Infos()
        {
            var current = FilePath.OS.Current;
            WriteLine($"FilePath");

            WriteLine($"  Current {FilePath.OS.Current}");
            WriteLine($"  Temp {FilePath.OS.Temp}");
            WriteLine($"  Random {FilePath.OS.Random}");

            WriteLine($"  Current.Value {current.Value}");
            WriteLine($"  Current.NormalizeValue {current.NormalizeValue}");
            WriteLine($"  Current.FileName {current.FileName}");
            WriteLine($"  Current.FileNameWithoutExtension {current.FileNameWithoutExtension}");
            WriteLine($"  Current.Extension {current.Extension}");
            WriteLine($"  Current.DirectoryName {current.DirectoryName}");
            WriteLine($"  Current.Directory {current.Directory}");
        }

        [Fact]
        public void DirectoryPath_Infos()
        {
            var current = DirectoryPath.OS.Current;
            WriteLine($"DirectoryPath");

            WriteLine($"  Current {DirectoryPath.OS.Current}");
            WriteLine($"  Data {DirectoryPath.OS.Data}");
            WriteLine($"  Temp {DirectoryPath.OS.Temp}");
            WriteLine($"  dotnet {DirectoryPath.OS.dotnet}");
            WriteLine($"  dotnettools {DirectoryPath.OS.dotnettools}");
            WriteLine($"  nuget {DirectoryPath.OS.nuget}");

            WriteLine($"  Current.Value {current.Value}");
            WriteLine($"  Current.NormalizeValue {current.NormalizeValue}");
            WriteLine($"  Current.DirectoryRoot {current.DirectoryRoot}");
            WriteLine($"  Current.LogicalDrives {current.LogicalDrives.ToJoinString(",")}");
        }

        [Fact]
        public void DirectoryPath_Os_SpecialFolderPath()
        {
            foreach (var name in fluent.Enum.Names<Environment.SpecialFolder>())
            {
                var value = fluent.Enum.TryParse<Environment.SpecialFolder>(name);
                WriteLine($"{name}{" ".Repeat(2)} => [{value}\\{(int)value}]{Environment.NewLine}  {DirectoryPath.OS.SpecialFolderPath(value)}");
            }
        }

        [Fact]
#pragma warning disable IDE1006 // Naming Styles
        public void fluent_OS()
#pragma warning restore IDE1006 // Naming Styles
        {
            WriteLine($"fluent.OS");
            WriteLine($"CurrentPlatform {fluent.OS.CurrentPlatform}");
            WriteLine($"IsWindows {fluent.OS.IsWindows}");
            WriteLine($"IsOSX {fluent.OS.IsOSX}");
            WriteLine($"IsLinux {fluent.OS.IsLinux}");
            WriteLine($"FrameworkDescription {fluent.OS.FrameworkDescription}");
            WriteLine($"OSArchitecture {fluent.OS.OSArchitecture}");
            WriteLine($"OSDescription {fluent.OS.OSDescription}");
            WriteLine($"ProcessArchitecture {fluent.OS.ProcessArchitecture}");
        }

        [Fact]
#pragma warning disable IDE1006 // Naming Styles
        public void fluent_App()
#pragma warning restore IDE1006 // Naming Styles
        {
            WriteLine($"fluent.App");
            WriteLine(fluent.Executable);
            WriteLine(fluent.Executable);

            WriteLine(fluent.App.ToJson());
            WriteLine(fluent.App.CustomAttributesAsJson());
            WriteLine($"fluent.FluentLib");
            WriteLine(fluent.FluentLib.ToJson());
            WriteLine(fluent.FluentLib.CustomAttributesAsJson());
        }
    }
}
