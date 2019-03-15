﻿using System;
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

            Assert.True(filePath.SplitValues.Count > 0);
            Assert.Equal("Test.txt", filePath.SplitValuesLast);

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

        [Fact]
        public void CloneTest()
        {
            var TestTextPath = new FilePath(DirectoryPath.OS.Temp, "Test.txt");
            {
                var CloneTestTextPath = TestTextPath.Clone();
                Assert.Equal(TestTextPath.Value, CloneTestTextPath.Value);
                Assert.Equal(TestTextPath.Extension, CloneTestTextPath.Extension);
                Assert.Equal(TestTextPath.FileName, CloneTestTextPath.FileName);
                Assert.Equal(TestTextPath.FileNameWithoutExtension, CloneTestTextPath.FileNameWithoutExtension);
                Assert.Equal(TestTextPath.FullPath, CloneTestTextPath.FullPath);
                Assert.Equal(TestTextPath.HasExtension, CloneTestTextPath.HasExtension);
                Assert.Equal((string)TestTextPath, (string)CloneTestTextPath);
            }
            {
                var CloneTestTextPath = TestTextPath.Clone("Clone.cln");
                Assert.Equal(TestTextPath.DirectoryName, CloneTestTextPath.DirectoryName);
                Assert.Equal((string)TestTextPath.Directory, (string)CloneTestTextPath.Directory);
                Assert.Equal((string)DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), TestTextPath.DirectoryName);
                Assert.Equal("Test.txt", TestTextPath.FileName);
                Assert.Equal("Test", TestTextPath.FileNameWithoutExtension);
                Assert.Equal(".txt", TestTextPath.Extension);
                Assert.Equal("Clone.cln", CloneTestTextPath.FileName);
                Assert.Equal("Clone", CloneTestTextPath.FileNameWithoutExtension);
                Assert.Equal(".cln", CloneTestTextPath.Extension);
            }
        }
    }
}
