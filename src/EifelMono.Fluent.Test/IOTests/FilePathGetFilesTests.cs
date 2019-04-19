using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.IOTests
{
    public class FilePathGetFilesTests : XunitCore
    {
        public static DirectoryPath s_srcFolder = new DirectoryPath(@".\..\..\..\..\..\src");
        public FilePathGetFilesTests(ITestOutputHelper output) : base(output) { }


        private void SearchFiles(string searchMask, int count = -1)
        {
            WriteLine($"SearchMask={searchMask} {DateTime.Now}");
            var startDirectory = s_srcFolder.MakeAbsolute();
            WriteLine($"Directory={startDirectory}");
            var files = startDirectory.GetFiles(searchMask);
            //foreach (var file in files)
            //    WriteLine(file);
            WriteLine($"Count={files.Count}");
            if (count != -1)
                Assert.Equal(count, files.Count);
        }
        [Fact]
        public void GetFilesInSrc()
        {
            SearchFiles("**/*.cs");
            SearchFiles("**/*.dll");
            SearchFiles("**/*.dll,*.cs");
            SearchFiles("**/EifelMono.Fluent/**/*.cs");
            SearchFiles("**/EifelMono.Fluent/**/*.dll");
            SearchFiles("**/EifelMono.Fluent/**/*.dll,*.cs");
        }
    }
}
