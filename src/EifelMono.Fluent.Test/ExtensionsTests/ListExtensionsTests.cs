using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;
using System.Linq;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class ListExtensionsTests : XunitCore
    {
        public ListExtensionsTests(ITestOutputHelper output) : base(output) { }


        [Fact]
        public void ListTests()
        {
            var list = new List<string>
            {
                "a"
            };
            Assert.Contains("a", list);
            var b = "b";
            Assert.Equal(b, list.AddValueReturnValue(b));
            Assert.Contains("b", list);
            Assert.Equal(b, list.RemoveValueReturnValue(b));
            Assert.DoesNotContain("b", list);

            Assert.Equal(list, list.AddValueReturnList("c"));
            Assert.Contains("c", list);
            Assert.Equal(list, list.RemoveValueReturnList("c"));
            Assert.DoesNotContain("c", list);

            var def = new List<string> { "d", "e", "f" };
            Assert.Equal(def, list.AddValuesReturnValues(def));
            Assert.Contains("d", list);
            Assert.Contains("e", list);
            Assert.Contains("f", list);
            Assert.Equal(def, list.RemoveValuesReturnValues(def));
            Assert.DoesNotContain("d", list);
            Assert.DoesNotContain("e", list);
            Assert.DoesNotContain("f", list);

            Assert.Equal(list, list.AddValuesReturnList(def));
            Assert.Contains("d", list);
            Assert.Contains("e", list);
            Assert.Contains("f", list);
            Assert.Equal(list, list.RemoveValuesReturnList(def));
            Assert.DoesNotContain("d", list);
            Assert.DoesNotContain("e", list);
            Assert.DoesNotContain("f", list);

            {
                var results = list.AddValuesReturnValues("d", "e", "f").ToList();
                Assert.Contains("d", results);
                Assert.Contains("e", results);
                Assert.Contains("f", results);
            }
            {
                var results = list.RemoveValuesReturnValues("d", "e", "f");
                Assert.Contains("d", results);
                Assert.Contains("e", results);
                Assert.Contains("f", results);
            }

            {
                var results = list.AddValuesReturnList("d", "e", "f").ToList();
                Assert.Contains("a", results);
                Assert.Contains("d", results);
                Assert.Contains("e", results);
                Assert.Contains("f", results);
            }
            {
                var results = list.RemoveValuesReturnList("d", "e", "f");
                Assert.Contains("a", results);
                Assert.DoesNotContain("d", results);
                Assert.DoesNotContain("e", results);
                Assert.DoesNotContain("f", results);
            }
        }
    }
}
