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
            var filePath = new FilePath(DirectoryPath.OS.Temp, "Test.txt");
            {
                var cloneFilePath = filePath.Clone();
                Assert.Equal(filePath.Value, cloneFilePath.Value);
                Assert.Equal(filePath.Extension, cloneFilePath.Extension);
                Assert.Equal(filePath.FileName, cloneFilePath.FileName);
                Assert.Equal(filePath.FileNameWithoutExtension, cloneFilePath.FileNameWithoutExtension);
                Assert.Equal(filePath.FullPath, cloneFilePath.FullPath);
                Assert.Equal(filePath.HasExtension, cloneFilePath.HasExtension);
                Assert.Equal((string)filePath, (string)cloneFilePath);
            }
            {
                var cloneFilePath = filePath.Clone("Clone.cln");
                Assert.Equal(filePath.DirectoryName, cloneFilePath.DirectoryName);
                Assert.Equal((string)filePath.Directory, (string)cloneFilePath.Directory);
                Assert.Equal((string)DirectoryPath.OS.Temp.IfEndsWithPathThenRemove(), filePath.DirectoryName);
                Assert.Equal("Test.txt", filePath.FileName);
                Assert.Equal("Test", filePath.FileNameWithoutExtension);
                Assert.Equal(".txt", filePath.Extension);
                Assert.Equal("Clone.cln", cloneFilePath.FileName);
                Assert.Equal("Clone", cloneFilePath.FileNameWithoutExtension);
                Assert.Equal(".cln", cloneFilePath.Extension);
            }
        }

        [Fact]
        public void CloneTestDateTime()
        {
            var TestTextFilePath = new FilePath(DirectoryPath.OS.Temp, "Test.txt");
            {
                {
                    var cloneFilePath = TestTextFilePath.Clone().ChangeFileNameWithoutExtensionAppend(FilePath.DateTimeFormat.yyyyMMdd);
                    if (cloneFilePath.FileWileNameWithoutExtensionLastAsDateTime() is var result && result.Ok)
                        Assert.Equal(DateTime.Now.Date, result.Value.Date);
                    else
                        AssertFail();
                }
                {
                    var cloneFilePath = TestTextFilePath.Clone().ChangeFileNameWithoutExtensionAppend(FilePath.DateTimeFormat.HHmmss);
                    if (cloneFilePath.FileWileNameWithoutExtensionLastAsDateTime() is var result && result.Ok)
                        Assert.True((DateTime.Now.TimeOfDay - result.Value.TimeOfDay).TotalSeconds < 1);
                    else
                        AssertFail();
                }

                {
                    var cloneFilePath = TestTextFilePath.Clone().ChangeFileNameWithoutExtensionAppend(FilePath.DateTimeFormat.yyyyMMddHHmmss);
                    if (cloneFilePath.FileWileNameWithoutExtensionLastAsDateTime() is var result && result.Ok)
                        Assert.True((DateTime.Now - result.Value).TotalSeconds < 1);
                    else
                        AssertFail();
                }
            }
        }

        [Fact]
        public void FilePath_Infos_From_TestFile()
        {
            var testFile = DirectoryPath.OS.Temp.CloneToFilePath($"{nameof(FilePath_Infos_From_TestFile)}.json");
            testFile.IfExists.ClearAttributes();
            testFile.IfExists.Do(f => { });
            // testFile.RemoveAttributesIfExist().DeleteIfExist();
            testFile.IfExists.Delete();

            Assert.False(testFile.Exists);

            testFile.WriteJson("Hello");

            Assert.True(testFile.Exists);
            var hello = testFile.ReadJson<string>();
            var a = testFile.ReadAllBytes();
            Assert.NotNull(a);
            var b = testFile.ReadAllLines();
            Assert.NotNull(b);
            var c = testFile.ReadAllText();
            Assert.NotNull(c);
            var d = testFile.ReadLines();
            Assert.NotNull(d);


            Assert.Equal("Hello", hello);

            WriteLine($"FilePath");

            WriteLine($"  Current {testFile}");

            WriteLine($"  Current.AttributeArchive {testFile.AttributeArchive}");
            WriteLine($"  Current.AttributeCompressed {testFile.AttributeCompressed}");
            WriteLine($"  Current.AttributeDevice {testFile.AttributeDevice}");
            WriteLine($"  Current.AttributeDirectory {testFile.AttributeDirectory}");
            WriteLine($"  Current.AttributeEncrypted {testFile.AttributeEncrypted}");
            WriteLine($"  Current.AttributeHidden {testFile.AttributeHidden}");
            WriteLine($"  Current.AttributeIntegrityStream {testFile.AttributeIntegrityStream}");
            WriteLine($"  Current.AttributeNormal {testFile.AttributeNormal}");
            WriteLine($"  Current.AttributeNoScrubData {testFile.AttributeNoScrubData}");
            WriteLine($"  Current.AttributeNotContentIndexed {testFile.AttributeNotContentIndexed}");
            WriteLine($"  Current.AttributeOffline {testFile.AttributeOffline}");
            WriteLine($"  Current.AttributeReadOnly {testFile.AttributeReadOnly}");
            WriteLine($"  Current.AttributeReparsePoint {testFile.AttributeReparsePoint}");
            WriteLine($"  Current.AttributeSparseFile {testFile.AttributeSparseFile}");
            WriteLine($"  Current.AttributeSystem {testFile.AttributeSystem}");
            WriteLine($"  Current.AttributeTemporary {testFile.AttributeTemporary}");

            WriteLine($"  Current.CreationTime {testFile.CreationTime}");
            WriteLine($"  Current.CreationTimeUtc {testFile.CreationTimeUtc}");
            WriteLine($"  Current.LastAccessTime {testFile.LastAccessTime}");
            WriteLine($"  Current.LastAccessTimeUtc {testFile.LastAccessTimeUtc}");
            WriteLine($"  Current.LastWriteTime {testFile.LastWriteTime}");
            WriteLine($"  Current.LastWriteTimeUtc {testFile.LastWriteTimeUtc}");

            Assert.False(testFile.AttributeHidden);
            testFile.AttributeHidden = true;
            Assert.True(testFile.AttributeHidden);
            testFile.AttributeHidden = false;
            Assert.False(testFile.AttributeHidden);

            testFile.AttributeHidden = true;
            Assert.True(testFile.AttributeHidden);
            testFile.AttributeHidden = false;
            Assert.False(testFile.AttributeHidden);

        }
    }
}
