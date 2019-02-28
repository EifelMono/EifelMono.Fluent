using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class InformationTest : XunitCore
    {
        public InformationTest(ITestOutputHelper output) : base(output) { }
        [Fact]
        public void DirectoryPath_Infos()
        {
            var current = DirectoryPath.Os.Current;
            WriteLine($"DirectoryPath");

            WriteLine($"  Current {DirectoryPath.Os.Current}");
            WriteLine($"  Data {DirectoryPath.Os.Data}");
            WriteLine($"  Temp {DirectoryPath.Os.Temp}");

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
