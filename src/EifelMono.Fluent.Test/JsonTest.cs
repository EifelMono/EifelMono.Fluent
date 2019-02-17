using System;
using System.Collections.Generic;
using System.Text;
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
            var x = new
            {
                A = new FilePath("./src"),
                B = "String"
            };

            var xs = x.ToJson();
            WriteLine(xs);
        }
    }
}
