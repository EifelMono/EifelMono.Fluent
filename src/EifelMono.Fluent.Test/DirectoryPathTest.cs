using System;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;
using System.Threading.Tasks;
using System.Threading;

namespace EifelMono.Fluent.Test
{
    public class DirectoryPathTest : XunitCore
    {
        public static DirectoryPath s_srcPath = new DirectoryPath(@".\..\..\..\..\..\src");
        public static DirectoryPath s_TestFluentPath = DirectoryPath.OS.Temp.Clone("TestFluent")
            .EnsureExist();

        public static int DirMax = 2;
        public static string DirName1 = "ABCDEF";
        public static string DirName2 = "GHIJKL";

        public DirectoryPathTest(ITestOutputHelper output) : base(output) { }
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
            WriteLine($"{s_TestFluentPath} {dirMax} {dirName}");
            for (int a = 1; a <= dirMax; a++)
            {
                var aDirectory = s_TestFluentPath.Clone(dirName.Repeat(a)).EnsureExist();
                for (int b = 1; b <= dirMax; b++)
                {
                    var bDirectory = aDirectory.Clone(dirName.Repeat(b)).EnsureExist();
                    for (int c = 1; c <= dirMax; c++)
                    {
                        var cDirectory = bDirectory.Clone(dirName.Repeat(c)).EnsureExist();
                    }
                }
            }
        }
        async Task DeleteTestDirectoriesAsync()
        {
            await s_TestFluentPath.CleanAsync().ConfigureAwait(false);
            await s_TestFluentPath.DeleteAsync();
        }


        [Theory(Skip ="Only single Test")]
        // [Theory]
        [InlineData(1, "ABCDEF")]

        public async void TestCreateDirectories(int dirMax, string dirName)
        {
            using var xlock= await new XlockDirectory().WaitAsync(s_TestFluentPath);
            WriteLine(s_TestFluentPath);
            await CreateTestDirectoriesAsync(dirMax, dirName);
            var dirs = s_TestFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(1 + (dirMax + dirMax * dirMax + dirMax * dirMax * dirMax), dirs.Count);
        }

        [Fact]
        public async void TestDeleteDirectories()
        {
            using var xlock = await new XlockDirectory().WaitAsync(s_TestFluentPath);
            WriteLine(s_TestFluentPath);
            await DeleteTestDirectoriesAsync();
            var dirs = s_TestFluentPath.GetDirectories("**");
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
            using var xlock = await new XlockDirectory().WaitAsync(s_TestFluentPath);
            await DeleteTestDirectoriesAsync();
            var dirs = s_TestFluentPath.GetDirectories("**");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Empty(dirs);
            await CreateTestDirectoriesAsync(dirMax, dirName);
            dirs = s_TestFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(1 + (dirMax + dirMax * dirMax + dirMax * dirMax * dirMax), dirs.Count);
            dirs = s_TestFluentPath.GetDirectories("**/A*");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(dirMax, dirs.Count);
            dirs = s_TestFluentPath.GetDirectories("**/*A*");
            WriteLine($"dirs={dirs.Count}");
            Assert.Equal(dirMax, dirs.Count);
            dirs = s_TestFluentPath.GetDirectories("**/*B*");
            WriteLine($"dirs={dirs.Count}");
            Assert.Equal(dirMax, dirs.Count);
            dirs = s_TestFluentPath.GetDirectories("**/A*/**");
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
