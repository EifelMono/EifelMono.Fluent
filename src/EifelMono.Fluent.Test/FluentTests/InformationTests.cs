﻿using System;
using EifelMono.Fluent.DotNet;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FluentTests
{
#pragma warning disable IDE1006 // Naming Styles
    public class InformationTests : XunitCore
    {
        public InformationTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ReadToString()
        {
            foreach (var type in typeof(fluent).Assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract)
                    try
                    {
                        var obj = Activator.CreateInstance(type);
                        obj.ToString();
                    }
                    catch { }
            }
        }
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

            foreach (var value in fluent.Enum.Values<Environment.SpecialFolder>())
            {
                var name = fluent.Enum.Name(value);
                WriteLine($"{name}{" ".Repeat(2)} => [{value}\\{(int)value}]{Environment.NewLine}  {DirectoryPath.OS.SpecialFolderPath(value)}");
            }
        }

        [Fact]
        public void fluent_OS()

        {
            WriteLine($"fluent.OS".NewLine());
            WriteLine($"CurrentPlatform {fluent.OSInfo.CurrentPlatform}");
            WriteLine($"IsWindows {fluent.OSInfo.IsWindows}");
            WriteLine($"IsOSX {fluent.OSInfo.IsOSX}");
            WriteLine($"IsLinux {fluent.OSInfo.IsLinux}");
            WriteLine($"FrameworkDescription {fluent.OSInfo.FrameworkDescription}");
            WriteLine($"OSArchitecture {fluent.OSInfo.OSArchitecture}");
            WriteLine($"OSDescription {fluent.OSInfo.OSDescription}");
            WriteLine($"ProcessArchitecture {fluent.OSInfo.ProcessArchitecture}");
            WriteLine($"fluent.OS.System {fluent.OS.System}");
        }

        [Fact]

        public void fluent_App()
        {
            WriteLine($"fluent.App".NewLine());
            WriteLine(fluent.Executable);
            WriteLine(fluent.Executable);

            WriteLine(fluent.App.ToJson());
            WriteLine(fluent.App.CustomAttributesAsJson());
            WriteLine($"fluent.FluentLib");
            WriteLine(fluent.FluentLib.ToJson());
            WriteLine(fluent.FluentLib.CustomAttributesAsJson());
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
