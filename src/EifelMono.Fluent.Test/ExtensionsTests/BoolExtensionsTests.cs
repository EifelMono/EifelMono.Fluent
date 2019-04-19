using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class BoolExtensionsTests : XunitCore
    {
        public BoolExtensionsTests(ITestOutputHelper output) : base(output) { }


        [Theory]
        [InlineData("true", true, true, true, true, true)]
        [InlineData("True", true, true, true, true, true)]
        [InlineData("false", false, true, true, true, true)]
        [InlineData("False", false, true, true, true, true)]
        [InlineData("aaa", default(bool), false, false, true, true)]
        public void DoTo(string textValue, bool convertValue, bool safeOk, bool doConvert, bool doTryConvert, bool doSafeConvert)
        {
            if (doConvert)
            {
                var result = textValue.ToBool();
                Assert.Equal(convertValue, result);
            }
            if (doTryConvert)
            {
                var result = textValue.ToBoolTry();
                Assert.Equal(convertValue, result);
            }
            if (doSafeConvert)
            {
                var (Ok, Value) = textValue.ToBoolSafe();
                Assert.Equal(safeOk, Ok);
                Assert.Equal(convertValue, Value);
            }
        }
    }
}
