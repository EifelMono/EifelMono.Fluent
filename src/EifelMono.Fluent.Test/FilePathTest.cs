using System;
using EifelMono.Fluent.IO;
using Xunit;
using System.IO;

namespace EifelMono.Fluent.Test
{
    public class FilePathTest
    {
        [Fact]
        public void OperationTestType()
        {
            var filePath = new FilePath("./src", "Karl.test");
            Assert.Equal(typeof(FilePath), filePath.GetType());

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
        }
    }
}
