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
    public class TriStateExtensionsTests : XunitCore
    {
        public TriStateExtensionsTests(ITestOutputHelper output) : base(output) { }

        [Theory]
        [InlineData(TriState.Yes, new object[] { TriState.Unkown, TriState.No }, false)]
        [InlineData(TriState.Yes, new object[] { TriState.Yes }, true)]
        public void IsTests(object value, object[] values, bool result)
        {
            Assert.Equal(result, value.Is(values));
            Assert.Equal(result, value.Is(values as IEnumerable<object>));
        }

        [Fact]
        public void TriStateTests()
        {
            var state = TriState.Unkown;
            Assert.True(state.IsUnkown());
            Assert.False(state.IsYes());
            Assert.False(state.IsNo());
            state = TriState.Yes;
            Assert.False(state.IsUnkown());
            Assert.True(state.IsYes());
            Assert.False(state.IsNo());
            state = TriState.No;
            Assert.False(state.IsUnkown());
            Assert.False(state.IsYes());
            Assert.True(state.IsNo());
        }
    }
}
