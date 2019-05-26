using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using System.Collections.Generic;

namespace EifelMono.Fluent.Test.FluentTests
{
    public class CoreTEsts : XunitCore
    {

        public CoreTEsts(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Fluent_Default()
        {
            {
                var result = fluent.Default<string>();
                Assert.True(result.GetType() == typeof(string));
                result = "hello word";
                WriteLine(result);
            }
            {
                var result = fluent.Default(typeof(string));
                Assert.True(result.GetType() == typeof(string));
                result = "hello word";
                WriteLine(result);
            }
            {
                var result = fluent.Default<int>();
                Assert.True(result.GetType() == typeof(int));
                result = 123;
                WriteLine(result);
            }
            {
                var result = fluent.Default(typeof(int));
                Assert.True(result.GetType() == typeof(int));
                result = 567;
                WriteLine(result);
            }

            {
                var result = fluent.Default<List<string>>();
                Assert.True(result.GetType() == typeof(List<string>));
                result.Add("hello word");
                WriteLine(result);
            }
            {
                var result = fluent.Default(typeof(List<string>));
                Assert.True(result.GetType() == typeof(List<string>));
                ((List<string>)result).Add("hello word");
                WriteLine(result);
            }

        }

    }
}
