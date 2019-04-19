using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class IntExtensionsTests : XunitCore
    {
        public IntExtensionsTests(ITestOutputHelper output) : base(output) { }


        [Theory]
        [InlineData("1", 1, true, true, true, true)]
        [InlineData("2", 2, true, true, true, true)]
        [InlineData("1234", 1234, true, true, true, true)]
        [InlineData("-1", -1, true, true, true, true)]
        [InlineData("asd", default(int), false, false, true, true)]
        public void DoTo(string textValue, int convertValue, bool safeOk, bool doConvert, bool doTryConvert, bool doSafeConvert)
        {
            if (doConvert)
            {
                var result = textValue.ToInt();
                Assert.Equal(convertValue, result);
            }
            if (doTryConvert)
            {
                var result = textValue.ToIntTry();
                Assert.Equal(convertValue, result);
            }
            if (doSafeConvert)
            {
                var (Ok, Value) = textValue.ToIntSafe();
                Assert.Equal(safeOk, Ok);
                Assert.Equal(convertValue, Value);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(-1, 1)]
        public void DoAbs(int value, int expectedValue)
        {
            Assert.Equal(expectedValue, value.Abs());
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(-1, 1, -1)]
        [InlineData(1, -1, -1)]
        public void DoMin(int value1, int value2, int expectedValue)
        {
            Assert.Equal(expectedValue, value1.Min(value2));
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(2, 1, 2)]
        [InlineData(-1, 1, 1)]
        [InlineData(1, -1, 1)]
        public void DoMax(int value1, int value2, int expectedValue)
        {
            Assert.Equal(expectedValue, value1.Max(value2));
        }

        [Theory]
        [InlineData(1, 1, 2, true)]
        [InlineData(2, 1, 2, true)]
        [InlineData(3, 1, 2, false)]
        [InlineData(-1, 1, 2, false)]
        [InlineData(10, 10, 20, true)]
        [InlineData(20, 10, 20, true)]
        [InlineData(30, 10, 20, false)]
        [InlineData(-10, 10, 20, false)]
        [InlineData(10, 20, 10, false)]
        public void DoRange(int value, int minValue, int maxValue, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.InRange(minValue, maxValue));
            Assert.Equal(expectedValue, value.InRangeOffset(minValue, maxValue - minValue));
        }


        [Theory]
        [InlineData(1, 1, 2, 1)]
        [InlineData(0, 1, 2, 1)]
        [InlineData(3, 1, 2, 2)]
        [InlineData(10, 10, 20, 10)]
        [InlineData(11, 10, 20, 11)]
        [InlineData(15, 10, 20, 15)]
        [InlineData(19, 10, 20, 19)]
        [InlineData(0, 10, 20, 10)]
        [InlineData(30, 10, 20, 20)]

        public void DoClamp(int value, int minValue, int maxValue, int expectedValue)
        {
            Assert.Equal(expectedValue, value.Clamp(minValue, maxValue));
        }
    }
}
