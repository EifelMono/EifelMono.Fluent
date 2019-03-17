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

        enum TestEnumA
        {
            Unknow,
            A,
            B
        }

        [Fact]
        public void Fluent_Enum_Values_TestEnumA()
        {

            var values = fluent.Enum.Values<TestEnumA>().ToList();
            Assert.Equal(3, values.Count);
            Assert.Contains(TestEnumA.Unknow, values);
            Assert.Contains(TestEnumA.A, values);
            Assert.Contains(TestEnumA.B, values);

            var names = fluent.Enum.Names<TestEnumA>().ToList();
            Assert.Equal(3, values.Count);
            Assert.Contains(TestEnumA.Unknow.ToString(), names);
            Assert.Contains(TestEnumA.A.ToString(), names);
            Assert.Contains(TestEnumA.B.ToString(), names);

            Assert.Equal(values.Count, names.Count);

            Assert.True(fluent.Enum.IsDefined<TestEnumA>(TestEnumA.Unknow));
            Assert.True(fluent.Enum.IsDefined<TestEnumA>(TestEnumA.A));
            Assert.True(fluent.Enum.IsDefined<TestEnumA>(TestEnumA.B));
            Assert.Equal(0, (int)TestEnumA.Unknow);
            Assert.Equal(1, (int)TestEnumA.A);
            Assert.Equal(2, (int)TestEnumA.B);
            Assert.True(fluent.Enum.IsDefined<TestEnumA>(0));
            Assert.True(fluent.Enum.IsDefined<TestEnumA>(1));
            Assert.True(fluent.Enum.IsDefined<TestEnumA>(2));
            Assert.False(fluent.Enum.IsDefined<TestEnumA>(3));
            Assert.True(fluent.Enum.Value<TestEnumA>(2).Ok);
            Assert.Equal(TestEnumA.B, fluent.Enum.Value<TestEnumA>(2).Value);
            Assert.True(fluent.Enum.Name<TestEnumA>(2).Ok);
            Assert.Equal(TestEnumA.B.ToString(), fluent.Enum.Name<TestEnumA>(2).Value);
            Assert.False(fluent.Enum.Name<TestEnumA>(3).Ok);
        }

        enum TestEnumB
        {
            Unknow= 1,
            A= 3,
            B= 5,
        }

        [Fact]
        public void Fluent_Enum_Values_TestEnumB()
        {

            var values = fluent.Enum.Values<TestEnumB>().ToList();
            Assert.Equal(3, values.Count);
            Assert.Contains(TestEnumB.Unknow, values);
            Assert.Contains(TestEnumB.A, values);
            Assert.Contains(TestEnumB.B, values);

            var names = fluent.Enum.Names<TestEnumB>().ToList();
            Assert.Equal(3, values.Count);
            Assert.Contains(TestEnumB.Unknow.ToString(), names);
            Assert.Contains(TestEnumB.A.ToString(), names);
            Assert.Contains(TestEnumB.B.ToString(), names);

            Assert.Equal(values.Count, names.Count);

            Assert.True(fluent.Enum.IsDefined<TestEnumB>(TestEnumB.Unknow));
            Assert.True(fluent.Enum.IsDefined<TestEnumB>(TestEnumB.A));
            Assert.True(fluent.Enum.IsDefined<TestEnumB>(TestEnumB.B));
            Assert.Equal(1, (int)TestEnumB.Unknow);
            Assert.Equal(3, (int)TestEnumB.A);
            Assert.Equal(5, (int)TestEnumB.B);
            Assert.True(fluent.Enum.IsDefined<TestEnumB>(1));
            Assert.True(fluent.Enum.IsDefined<TestEnumB>(3));
            Assert.True(fluent.Enum.IsDefined<TestEnumB>(5));
            Assert.False(fluent.Enum.IsDefined<TestEnumB>(0));
            Assert.True(fluent.Enum.Value<TestEnumB>(5).Ok);
            Assert.Equal(TestEnumB.B, fluent.Enum.Value<TestEnumB>(5).Value);
            Assert.True(fluent.Enum.Name<TestEnumB>(5).Ok);
            Assert.Equal(TestEnumB.B.ToString(), fluent.Enum.Name<TestEnumB>(5).Value);
            Assert.False(fluent.Enum.Name<TestEnumB>(0).Ok);
        }
    }
}
