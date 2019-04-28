using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using System.Linq;
using Xunit.Abstractions;
using System.Collections;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class LinqExtensionsTests : XunitCore
    {
        public LinqExtensionsTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ForEachTests()
        {
            var items = Enumerable.Range(0, 100);
            var count1 = 0;
            foreach (var item in items)
                count1 += item;

            Assert.Equal(count1, items.Sum());

            var count2 = 0;
            // System.Collections.Generic.IList
            items.ToList().ForEach(item => count2 += item);

            Assert.Equal(count2, count1);

            var count3 = 0;
            // EifelMono.Fluent.Extensions.LinqExtensions
            items.ForEach(item => count3 += item);
            Assert.Equal(count3, count1);
            Assert.Equal(count3, count2);

            var count4 = 0;
            var count5 = 0;
            // EifelMono.Fluent.Extensions.LinqExtensions
            items.ForEachIndexed((item, index) =>
            {
                count4 += item;
                count5 += index;
            });
            Assert.Equal(count4, count1);
            Assert.Equal(count4, count2);
            Assert.Equal(count4, count3);

            Assert.Equal(count5, count1);
            Assert.Equal(count5, count2);
            Assert.Equal(count5, count3);
        }
    }
}
