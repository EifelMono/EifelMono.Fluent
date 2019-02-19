using System;
using EifelMono.Fluent.IO;
using Xunit;
using System.IO;
using Xunit.Abstractions;
using System.Linq;

namespace EifelMono.Fluent.Test
{
    public class DirectoryPathTest : XunitCore
    {
        public DirectoryPathTest(ITestOutputHelper output) : base(output) { }
        [Fact]
        public void SpecialFolderOutput()
        {
            foreach (var value in fluent.Enum.Values<Environment.SpecialFolder>())
                WriteLine($"{value}{Environment.NewLine}  {DirectoryPath.Os.SpecialFolderPath(value)}");
        }
        [Fact]
        public void SpecialFolderOutputNames()
        {
            var valueNames = fluent.Enum.Values<Environment.SpecialFolder>()
                    .Select(v => v.ToString());
            var names = fluent.Enum.Names<Environment.SpecialFolder>();
        }
        [Fact]
        public void OperationTestType()
        {
            var dirPath = new DirectoryPath("./src");
            Assert.Equal(typeof(DirectoryPath), dirPath.GetType());

            {
                var v = dirPath;
                Assert.Equal(typeof(DirectoryPath), v.GetType());
            }

            {
#pragma warning disable IDE0007 // Use implicit type
                DirectoryPath v = dirPath;
#pragma warning restore IDE0007 // Use implicit type
                Assert.Equal(typeof(DirectoryPath), v.GetType());
            }

            {
                string v = dirPath;
                Assert.Equal(typeof(string), v.GetType());
            }
        }

        [Fact]
        public void FindFiles()
        {
            var dir = new DirectoryPath(@"C:\Dev\github\EifelMono.Fluent\src");
            foreach (var f in dir.GetFiles(@"**\*.cs"))
                WriteLine(f);
        }
    }
}
