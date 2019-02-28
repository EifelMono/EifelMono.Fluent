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
            var current = FilePath.Os.Current;
            WriteLine($"FilePath");

            WriteLine($"  Current {FilePath.Os.Current}");
            WriteLine($"  Temp {FilePath.Os.Temp}");
            WriteLine($"  Random {FilePath.Os.Random}");

            WriteLine($"  Current.Value {current.Value}");
            WriteLine($"  Current.NormalizedValue {current.NormalizedValue}");
            WriteLine($"  Current.FileName {current.FileName}");
            WriteLine($"  Current.FileNameWithoutExtension {current.FileNameWithoutExtension}");
            WriteLine($"  Current.Extension {current.Extension}");
            WriteLine($"  Current.DirectoryName {current.DirectoryName}");
            WriteLine($"  Current.Directory {current.Directory}");

        }

        [Fact]
        public void DirectoryPath_Infos()
        {
            var current = DirectoryPath.Os.Current;
            WriteLine($"DirectoryPath");

            WriteLine($"  Current {DirectoryPath.Os.Current}");
            WriteLine($"  Data {DirectoryPath.Os.Data}");
            WriteLine($"  Temp {DirectoryPath.Os.Temp}");

            WriteLine($"  Current.Value {current.Value}");
            WriteLine($"  Current.NormalizedValue {current.NormalizedValue}");
            WriteLine($"  Current.DirectoryRoot {current.DirectoryRoot}");
            WriteLine($"  Current.LogicalDrives {current.LogicalDrives.ToJoinString(",")}");

        }

        [Fact]
        public void DirectoryPath_Os_SpecialFolderPath()
        {
            foreach (var value in fluent.Enum.Values<Environment.SpecialFolder>())
                WriteLine($"{value}{Environment.NewLine}  {DirectoryPath.Os.SpecialFolderPath(value)}");
        }

    }
}
