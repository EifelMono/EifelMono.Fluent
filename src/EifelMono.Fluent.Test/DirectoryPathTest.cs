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

        void CreateTestFluent(int max = 2)
        {
            var aName = "ABCDEF";
            for (int a = 1; a <= max; a++)
            {
                var aDirectory = s_TestFluentPath.Clone(aName.Repeat(a)).EnsureExist();
                for (int b = 1; b <= max; b++)
                {
                    var bDirectory = aDirectory.Clone(aName.Repeat(b)).EnsureExist();
                    for (int c = 1; c <= max; c++)
                    {
                        var cDirectory = bDirectory.Clone(aName.Repeat(c)).EnsureExist();
                    }
                }
            }
        }
        async Task DeleteTestFluentAsync()
        {
            await s_TestFluentPath.CleanAsync().ConfigureAwait(false);
            s_TestFluentPath.Delete();
            // The release delete needs time
            // await Task.Delay(100).ConfigureAwait(false);
        }

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

        [Fact]
        public void CreateTestDir_Test()
        {
            var max = 5;
            WriteLine(s_TestFluentPath);
            CreateTestFluent(max);
            var dirs = s_TestFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
            Assert.Equal(1 + (max + max * max + max * max * max), dirs.Count);
        }

        [Fact]
        public async void DeleteTestDir_Test()
        {
            WriteLine(s_TestFluentPath);
            await DeleteTestFluentAsync();
            var dirs = s_TestFluentPath.GetDirectories("**");
            WriteLine($"dirs={dirs.Count}");
            foreach (var dir in dirs)
                WriteLine(dir);
          
            Assert.Empty(dirs);
        }
        private void SearchDirectory(string searchMask, int count = -1)
        {
            WriteLine($"SearchMask={searchMask} {DateTime.Now}");
            var startDirectory = s_srcPath.MakeAbsolute();
            WriteLine($"Directory={startDirectory}");
            var foundDirectories = startDirectory.GetDirectories(searchMask);
            //foreach (var directory in foundDirectories)
            //    WriteLine(directory);
            WriteLine($"Count={foundDirectories.Count} expected={count}");
            //if (count != -1)
            //    Assert.Equal(count, foundDirectories.Count);
        }
        [Fact]
        public void GetDir1Async()
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
