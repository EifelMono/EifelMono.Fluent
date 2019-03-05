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
        public void TypeTest()
        {
            var filePath = new FilePath(DirectoryPath.OS.Temp, "Test.txt");
            Assert.Equal(typeof(FilePath), filePath.GetType());

            var safeFilePath = filePath;
            Assert.Equal(filePath, safeFilePath);

            {
                string v = filePath;
                Assert.Equal(typeof(string), v.GetType());
            }

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
                string v = "./src/Test.txt".AsFilePath();
                Assert.Equal(typeof(string), v.GetType());
            }
        }

        [Fact]
        public void PropertyTest()
        {
            var filePath = new FilePath(DirectoryPath.OS.Temp, "Test.txt");

            Assert.Equal("Test.txt", filePath.FileName);
            Assert.Equal("Test", filePath.FileNameWithoutExtension);
            Assert.Equal(".txt", filePath.Extension);
            Assert.Equal(DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), filePath.DirectoryName);

            filePath.ChangeFileName("Hallo.xxx");
            Assert.Equal("Hallo.xxx", filePath.FileName);
            Assert.Equal("Hallo", filePath.FileNameWithoutExtension);
            Assert.Equal(".xxx", filePath.Extension);
            Assert.Equal(DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), filePath.DirectoryName);

            filePath.ChangeExtension(".json");
            Assert.Equal("Hallo.json", filePath.FileName);
            Assert.Equal("Hallo", filePath.FileNameWithoutExtension);
            Assert.Equal(".json", filePath.Extension);
            Assert.Equal(DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), filePath.DirectoryName);

            filePath.ChangeFileNameWithoutExtension("Test");
            Assert.Equal("Test.json", filePath.FileName);
            Assert.Equal("Test", filePath.FileNameWithoutExtension);
            Assert.Equal(".json", filePath.Extension);
            Assert.Equal(DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), filePath.DirectoryName);

            filePath.RemoveExtension();
            Assert.Equal("Test", filePath.FileName);
            Assert.Equal("Test", filePath.FileNameWithoutExtension);
            Assert.Equal("", filePath.Extension);
            Assert.Equal(DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), filePath.DirectoryName);
        }
    }
}
