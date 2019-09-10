using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using System.Collections.Generic;

namespace EifelMono.Fluent.Test.FluentTests
{
    public class CoreTests : XunitCore
    {

        public CoreTests(ITestOutputHelper output) : base(output) { }

#pragma warning disable xUnit1004 // Test methods should not be skipped
        [Fact(Skip = "No idee to cast")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
        public void Fluent_Cast()
        {
            //var a = new List<object>
            //{
            //    "1",
            //    "2"
            //};
            //var o = (object)a;
            //var b = o.GetType().MakeGenericType(typeof(List<string>));
            //Assert.Equal("1", b[0]);
            //Assert.Equal("2", b[0]);

        }
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
