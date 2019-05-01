using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class GenericExtensionsTests : XunitCore
    {
        public GenericExtensionsTests(ITestOutputHelper output) : base(output) { }


        [Theory]
        [InlineData(1, new object[] { 1, 2, 3 }, true)]
        [InlineData(1, new object[] { 2, 3 }, false)]
        [InlineData("Hello", new object[] { "Hello", "World"}, true)]
        [InlineData("hello", new object[] { "Hello", "World" }, false)]
        [InlineData(TriState.Yes, new object[] { TriState.Unkown, TriState.No }, false)]
        [InlineData(TriState.Yes, new object[] { TriState.Yes }, true)]
        public void IsTests(object value, object[] values, bool result)
        {
            Assert.Equal(result, value.Is(values));
            Assert.Equal(result, value.Is(values as IEnumerable<object>));
        }
    }
}
