using System;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class FilePathTest : XunitCore
    {
        public FilePathTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void OperationTestType()
        {
            var filePath = new FilePath("./src", "Karl.test");
            Assert.Equal(typeof(FilePath), filePath.GetType());

            {
                var v = filePath;
                Assert.Equal(typeof(FilePath), v.GetType());
            }

            {
#pragma warning disable IDE0007 // Use implicit type
                FilePath v = filePath;
#pragma warning restore IDE0007 // Use implicit type
                Assert.Equal(typeof(FilePath), v.GetType());
            }

            {
                string v = filePath;
                Assert.Equal(typeof(string), v.GetType());
            }

            {
                string v = "./src/Karl.Test".AsFilePath();
                Assert.Equal(typeof(string), v.GetType());
            }
        }

        [Fact]
        public void PropertyTest()
        {
            var filePath = new FilePath("./src", "Hugo.test");
            if (filePath.Exists)
                Console.WriteLine("x");
            Assert.Equal("./src".NormalizePath(), filePath.DirectoryName);
            Assert.Equal("Hugo.test", filePath.FileName);
            Assert.Equal("Hugo", filePath.FileNameWithoutExtension);
            Assert.Equal(".test", filePath.Extension);
            filePath.RemoveExtension();
            Assert.Equal("Hugo", filePath.FileName);
            Assert.Equal("Hugo", filePath.FileNameWithoutExtension);
            Assert.Equal("", filePath.Extension);
        }
    }
}
