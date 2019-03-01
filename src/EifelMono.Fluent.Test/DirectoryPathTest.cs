using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class DirectoryPathTest : XunitCore
    {
        public static DirectoryPath s_SrcFolder = new DirectoryPath(@".\..\..\..\..\..\src");
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

        private void SearchDirectory(string searchMask)
        {
            WriteLine($"SearchMask={searchMask}");
            var startDirectory = s_SrcFolder.MakeAbsolute();
            WriteLine($"Directory={startDirectory}");
            var foundDirectories = startDirectory.GetDirectories("**");
            foreach (var directory in foundDirectories)
                WriteLine(directory);
            WriteLine($"Count={foundDirectories.Count}");

        }
        [Fact]
        public void GetDir1Async()
        {
            SearchDirectory("**");
        }

        [Fact]
        public async void FindFilesAsync()
        {
            var dir = s_SrcFolder;
            foreach (var f in await dir.GetFilesAsync(@"**\*.cs"))
                WriteLine(f);
        }

        [Fact]
        public void FindFiles()
        {
            var dir = s_SrcFolder;
            foreach (var f in dir.GetFiles(@"**\*.cs"))
                WriteLine(f);
        }
    }
}
