using System;
using EifelMono.Fluent.IO;
using Xunit;

namespace EifelMono.Fluent.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var x = new DirectoryPath("./src");
            var s = x.FullPath;

            string a = x;
            var b = x;
            DirectoryPath c = x;

        }
    }
}
