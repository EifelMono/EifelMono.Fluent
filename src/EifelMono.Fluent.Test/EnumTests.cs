using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Test
{
    public class EnumTest : XunitCore
    {
        public EnumTest(ITestOutputHelper output) : base(output) { }
        [Fact]
        public void Fluent_Enum_Values_SpecialFolderOutputNames()
        {
            var valueNames = fluent.Enum.Values<Environment.SpecialFolder>()
                .Select(v => v.ToString()).ToList();
            var names = fluent.Enum.Names<Environment.SpecialFolder>().ToList();
            WriteLine(valueNames.ToJson());
            WriteLine(names.ToJson());
            Assert.Equal(valueNames.Count, names.Count);
        }
    }
}
