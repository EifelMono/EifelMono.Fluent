using System;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Test
{
    public class DirectoryPathTest : XunitCore
    {
        public static DirectoryPath s_srcFolder = new DirectoryPath(@".\..\..\..\..\..\src");
        public DirectoryPathTest(ITestOutputHelper output) : base(output) { }
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

        private void SearchDirectory(string searchMask, int count = -1)
        {
            WriteLine($"SearchMask={searchMask} {DateTime.Now}");
            var startDirectory = s_srcFolder.MakeAbsolute();
            WriteLine($"Directory={startDirectory}");
            var foundDirectories = startDirectory.GetDirectories(searchMask);
            foreach (var directory in foundDirectories)
                WriteLine(directory);
            WriteLine($"Count={foundDirectories.Count}");
            if (count != -1)
                Assert.Equal(count, foundDirectories.Count);
        }
        [Fact]
        public void GetDir1Async()
        {
            try
            {
                // 38
                // SearchDirectory("**");
                // 37 /34 ???
                SearchDirectory("**/*/**", 37);
                // SearchDirectory("**/EifelMono.Fluent/**");
                // 6
                // SearchDirectory("**/EifelMono.Fluent/*");
                // 4
                // SearchDirectory("**/EifelMono.Fluent.*/*");
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
            var dir = s_srcFolder;
            foreach (var f in await dir.GetFilesAsync(@"**\*.cs"))
                WriteLine(f);
        }

        [Fact]
        public void FindFiles()
        {
            var dir = s_srcFolder;
            foreach (var f in dir.GetFiles(@"**\*.cs"))
                WriteLine(f);
        }
    }
}
