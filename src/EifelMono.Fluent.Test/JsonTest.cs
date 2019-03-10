using System.Linq;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class JsonTest : XunitCore
    {
        public JsonTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void SerializeFilePath()
        {
            var directoryName = "directoryName";
            var fileName = "fileName";
            var x = new
            {
                A = new FilePath(fileName),
                B = new FilePath(directoryName, fileName),
                C = new FilePath(new DirectoryPath(directoryName), fileName),
                D = new DirectoryPath(directoryName),
                E = new DirectoryPath(new DirectoryPath(directoryName))
            };

            Assert.Equal(x.A.FileName, fileName);
            Assert.Equal(x.B.FileName, fileName);
            Assert.Equal(x.C.FileName, fileName);

            Assert.Equal(x.B.Directory.SplitValuesLast, directoryName);
            Assert.Equal(x.C.Directory.SplitValuesLast, directoryName);
            Assert.Equal(x.D.SplitValuesLast, directoryName);
            Assert.Equal(x.E.SplitValuesLast, directoryName);

            var json = x.ToJson();
            var jsonEnvelope = x.ToJsonEnvelope();
            WriteLine(json);
            WriteLine(jsonEnvelope);
        }
    }
}
