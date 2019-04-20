using System;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;
using System.Threading.Tasks;
using System.Threading;
using EifelMono.Fluent.Test.XunitTests;

namespace EifelMono.Fluent.Test.IOTests
{
    public class DirectoryPathTests : XunitCore
    {
        public static DirectoryPath s_srcPath = new DirectoryPath(@".\..\..\..\..\..\src");
        public static DirectoryPath s_testFluentPath = DirectoryPath.OS.Temp.Clone("TestFluent")
            .EnsureExist();

        public static int s_dirMax = 2;
        public static string s_dirName1 = "ABCDEF";
        public static string s_dirName2 = "GHIJKL";

        public DirectoryPathTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void OperatorTest()
        {
            DirectoryPath f = @"C:\";
            Assert.Equal(typeof(DirectoryPath), f.GetType());

            string s = f;
            Assert.Equal(@"C:\", s);
            Assert.Equal(@"C:\", f.Value);
        }
        [Fact]
        public void TypeTest()
        {
            var dirPath = new DirectoryPath("./src");
            Assert.Equal(typeof(DirectoryPath), dirPath.GetType());

            var safeDirPath = dirPath;
            Assert.Equal(dirPath, safeDirPath);

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

        async Task CreateTestDirectoriesAsync(int dirMax, string dirName)
        {
            await Task.Delay(1);
            WriteLine($"{s_testFluentPath} {dirMax} {dirName}");
            for (int a = 1; a <= dirMax; a++)
            {
                var aDirectory = s_testFluentPath.Clone(dirName.Repeat(a)).EnsureExist();
                for (int b = 1; b <= dirMax; b++)
                {
                    var bDirectory = aDirectory.Clone(dirName.Repeat(b)).EnsureExist();
                    for (int c = 1; c <= dirMax; c++)
                    {
#pragma warning disable IDE0059 // Value assigned to symbol is never used
                        var cDirectory = bDirectory.Clone(dirName.Repeat(c)).EnsureExist();
#pragma warning restore IDE0059 // Value assigned to symbol is never used
                    }
                }
            }
        }
        async Task DeleteTestDirectoriesAsync()
        {
            await s_testFluentPath.CleanAsync().ConfigureAwait(false);
            await s_testFluentPath.DeleteAsync();
        }


#pragma warning disable xUnit1004 // Test methods should not be skipped
        [Theory(Skip = "Only single Test")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
        // [Theory]
        [InlineData(1, "ABCDEF")]

        public async void TestCreateDirectories(int dirMax, string dirName)
        {
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            using var xlock = await new XlockDirectory().WaitAsync(s_testFluentPath);
#pragma warning restore IDE0059 // Value assigned to symbol is never used
            WriteLine(s_testFluentPath);
            await CreateTestDirectoriesAsync(dirMax, dirName);
            var dirs = s_testFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(1 + (dirMax + dirMax * dirMax + dirMax * dirMax * dirMax), dirs.Count);
        }

        [Fact]
        public async void TestDeleteDirectories()
        {
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            using var xlock = await new XlockDirectory().WaitAsync(s_testFluentPath);
#pragma warning restore IDE0059 // Value assigned to symbol is never used
            WriteLine(s_testFluentPath);
            await DeleteTestDirectoriesAsync();
            var dirs = s_testFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Empty(dirs);
        }

        [Theory]
        [InlineData(1, "ABCDEF")]
        [InlineData(3, "ABCDEF")]
        [InlineData(5, "ABCDEF")]
        [InlineData(2, "ABCDEF")]

        public async void TestCreateAndDeleteDirectories(int dirMax, string dirName)
        {
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            using var xlock = await new XlockDirectory().WaitAsync(s_testFluentPath);
#pragma warning restore IDE0059 // Value assigned to symbol is never used
            await DeleteTestDirectoriesAsync();
            var dirs = s_testFluentPath.GetDirectories("**");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Empty(dirs);
            await CreateTestDirectoriesAsync(dirMax, dirName);
            dirs = s_testFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(1 + (dirMax + dirMax * dirMax + dirMax * dirMax * dirMax), dirs.Count);
            dirs = s_testFluentPath.GetDirectories("**/A*");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(dirMax, dirs.Count);
            dirs = s_testFluentPath.GetDirectories("**/*A*");
            WriteLine($"dirs={dirs.Count}");
            Assert.Equal(dirMax, dirs.Count);
            dirs = s_testFluentPath.GetDirectories("**/*B*");
            WriteLine($"dirs={dirs.Count}");
            Assert.Equal(dirMax, dirs.Count);
            dirs = s_testFluentPath.GetDirectories("**/A*/**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(dirMax + dirMax * dirMax + dirMax * dirMax * dirMax, dirs.Count);
        }
        private void SearchDirectory(string searchMask, int count = -1)
        {
            WriteLine($"SearchMask={searchMask} {DateTime.Now}");
            var startDirectory = s_srcPath.MakeAbsolute();
            WriteLine($"Directory={startDirectory}");
            var foundDirectories = startDirectory.GetDirectories(searchMask);
            foreach (var directory in foundDirectories)
                WriteLine(directory);
            WriteLine($"Count={foundDirectories.Count} expected={count}");
            //if (count != -1)
            //    Assert.Equal(count, foundDirectories.Count);
        }
        [Fact]
        public void SrcCsSearchWithoutCheck()
        {
            try
            {
                SearchDirectory("**", 40);
                SearchDirectory("**/*/**", 39);
                SearchDirectory("**/EifelMono.Fluent/**", 21);
                SearchDirectory("**/EifelMono.Fluent/*", 8);
                SearchDirectory("**/EifelMono.Fluent.*/*", 4);
                SearchDirectory("**/*Test/**", 11);
                SearchDirectory("**/net*");
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
                Assert.True(false, ex.ToString());
            }
        }


        [Fact]
        public async void FindFilesAsync()
        {
            var dir = s_srcPath;
            foreach (var f in await dir.GetFilesAsync(@"**\*.cs"))
                WriteLine(f);
        }

        [Fact]
        public void FindFiles()
        {
            var dir = s_srcPath;
            foreach (var f in dir.GetFiles(@"**\*.cs"))
                WriteLine(f);
        }
    }
}
