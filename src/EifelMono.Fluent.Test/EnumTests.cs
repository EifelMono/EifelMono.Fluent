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

        enum TestEnum
        {
            Unknow,
            A,
            B
        }
        [Fact]
        public void Fluent_Enum_Values_TestEnums()
        {
            var values = fluent.Enum.Values<TestEnum>().ToList();
            Assert.Equal(3, values.Count);

            var names = fluent.Enum.Names<TestEnum>().ToList();
            Assert.Equal(3, values.Count);

            Assert.Equal(values.Count, names.Count);

            Assert.True(fluent.Enum.IsDefined<TestEnum>(TestEnum.Unknow));
            Assert.True(fluent.Enum.IsDefined<TestEnum>(TestEnum.A));
            Assert.True(fluent.Enum.IsDefined<TestEnum>(TestEnum.B));
            Assert.Equal(0, (int)TestEnum.Unknow);
            Assert.Equal(1, (int)TestEnum.A);
            Assert.Equal(2, (int)TestEnum.B);
            Assert.True(fluent.Enum.IsDefined<TestEnum>(0));
            Assert.True(fluent.Enum.IsDefined<TestEnum>(1));
            Assert.True(fluent.Enum.IsDefined<TestEnum>(2));
            Assert.False(fluent.Enum.IsDefined<TestEnum>(3));
            Assert.True(fluent.Enum.Value<TestEnum>(2).Ok);
            Assert.Equal(TestEnum.B, fluent.Enum.Value<TestEnum>(2).Value);
            Assert.True(fluent.Enum.Name<TestEnum>(2).Ok);
            Assert.Equal(TestEnum.B.ToString(), fluent.Enum.Name<TestEnum>(2).Value);
            Assert.False(fluent.Enum.Name<TestEnum>(3).Ok);
        }
    }
}
