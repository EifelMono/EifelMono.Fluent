using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class EnumExtensionsTests : XunitCore
    {
        public EnumExtensionsTests(ITestOutputHelper output) : base(output) { }

        private enum TestEnums
        {
            A,
            B,
            C,
            D,
            AA,
            BB,
            CC,
            DD
        }

        [Fact]
        public void TestFluent()
        {

        }


        [Fact]
        public void TestExtensions()
        {
            var v = TestEnums.A;
            Assert.Equal(TestEnums.A, v);

            v= v.Next();
            Assert.Equal(TestEnums.B, v);

            v= v.Last();
            Assert.Equal(TestEnums.DD, v);

            v = v.Previous();
            Assert.Equal(TestEnums.CC, v);

            v= v.First();
            Assert.Equal(TestEnums.A, v);
        }
    }
}
