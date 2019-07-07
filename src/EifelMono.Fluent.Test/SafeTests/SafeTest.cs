#if NETSTANDARD2_1_PLUS
using System;
using System.Runtime.CompilerServices;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Core;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.CoreTests
{

    public class SafeCoreTest : XunitCore
    {
        public SafeCoreTest(ITestOutputHelper output) : base(output) { }


        private (bool Ok, string Value) IsSafeTupleReturnTrueOk(ITuple thisValue)
        {
            if (new SafeTuple<string>(thisValue) is var safeTuple && !safeTuple.Ok)
                return (safeTuple.Ok, safeTuple.ThisValue);
            return (true, nameof(IsSafeTupleReturnTrueOk));
        }
        private (bool Ok, string Value) IsSafeTupleReturnFalseOk(ITuple thisValue)
        {
            if (new SafeTuple<string>(thisValue) is var safeTuple && !safeTuple.Ok)
                return (safeTuple.Ok, safeTuple.ThisValue);
            return (false, "");
        }


#pragma warning disable IDE0042 // Deconstruct variable declaration
        [Fact]
        public void SafeTuple_Test_Ok_Extensions()
        {
            {
                var start = (true, "Start");
                var result = IsSafeTupleReturnTrueOk(start);

                Assert.True(result.Ok);
                Assert.Equal(nameof(IsSafeTupleReturnTrueOk), result.Value);
            }
            {
                var start = (true, "Start");
                var result = IsSafeTupleReturnFalseOk(start);

                Assert.False(result.Ok);
                Assert.Equal("Start", result.Value);
            }
        }
#pragma warning restore IDE0042 // Deconstruct variable declaration
    }
}
#endif
