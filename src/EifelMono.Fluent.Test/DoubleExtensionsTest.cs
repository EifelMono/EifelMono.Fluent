using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Test
{
    public class DoubleExtensionsTest : XunitCore
    {
        public DoubleExtensionsTest(ITestOutputHelper output) : base(output) { }


        [Theory]
        [InlineData("1", 1, true, true, true, true)]
        [InlineData("2", 2, true, true, true, true)]
        [InlineData("1234", 1234, true, true, true, true)]
        [InlineData("-1", -1, true, true, true, true)]
        [InlineData("asd", default(double), false, false, true, true)]
        public void DoTo(string textValue, double convertValue, bool safeOk, bool doConvert, bool doTryConvert, bool doSafeConvert)
        {
            if (doConvert)
            {
                var result = textValue.ToDouble();
                Assert.Equal(convertValue, result);
            }
            if (doTryConvert)
            {
                var result = textValue.ToDoubleTry();
                Assert.Equal(convertValue, result);
            }
            if (doSafeConvert)
            {
                var result = textValue.ToDoubleSafe();
                Assert.Equal(safeOk, result.Ok);
                Assert.Equal(convertValue, result.Value);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(-1, 1)]
        public void DoAbs(double value, double expectedValue)
        {
            Assert.Equal(expectedValue, value.Abs());
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(-1, 1, -1)]
        [InlineData(1, -1, -1)]
        public void DoMin(double value1, double value2, double expectedValue)
        {
            Assert.Equal(expectedValue, value1.Min(value2));
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(2, 1, 2)]
        [InlineData(-1, 1, 1)]
        [InlineData(1, -1, 1)]
        public void DoMax(double value1, double value2, double expectedValue)
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
        public void DoRange(double value, double minValue, double maxValue, bool expectedValue)
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

        public void DoClamp(double value, double minValue, double maxValue, double expectedValue)
        {
            Assert.Equal(expectedValue, value.Clamp(minValue, maxValue));
        }
    }
}
