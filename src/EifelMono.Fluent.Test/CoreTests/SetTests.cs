using System;
using EifelMono.Fluent.Core;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.CoreTests
{

    public class SetTest : XunitCore
    {
        public SetTest(ITestOutputHelper output) : base(output) { }

        private void AllDays(Set<DayOfWeek> days)
        {
            Assert.True(days.Has(DayOfWeek.Monday));
            Assert.True(days.Has(DayOfWeek.Tuesday));
            Assert.True(days.Has(DayOfWeek.Wednesday));
            Assert.True(days.Has(DayOfWeek.Thursday));
            Assert.True(days.Has(DayOfWeek.Friday));
            Assert.True(days.Has(DayOfWeek.Saturday));
            Assert.True(days.Has(DayOfWeek.Sunday));

            Assert.True(days.Has(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday));
            Assert.True(days.Is(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday));
        }
        [Fact]
        public void CoreTests()
        {
            var days = new Set<DayOfWeek>();
            Assert.True(days.IsEmpty());

            days.Invert();
            AllDays(days);
            days.Invert();
            Assert.True(days.IsEmpty());
            Assert.True(days.Has());
            Assert.True(days.Is());
            days.Fill();
            AllDays(days);
            days.Clear();
            Assert.True(days.IsEmpty());
            Assert.True(days.Has());
            Assert.True(days.Is());

            days.Include(DayOfWeek.Monday);
            Assert.True(days.Has(DayOfWeek.Monday));
            Assert.False(days.Has(DayOfWeek.Friday));
            Assert.False(days.Is(DayOfWeek.Monday, DayOfWeek.Friday));
            days.Include(DayOfWeek.Friday);
            Assert.True(days.Has(DayOfWeek.Friday));
            Assert.False(days.Has(DayOfWeek.Wednesday));
            Assert.True(days.Is(DayOfWeek.Monday, DayOfWeek.Friday));
            Assert.False(days.Is(DayOfWeek.Monday, DayOfWeek.Friday, DayOfWeek.Wednesday));

            days.Invert();
            Assert.False(days.Has(DayOfWeek.Monday));
            Assert.True(days.Has(DayOfWeek.Tuesday));
            Assert.True(days.Has(DayOfWeek.Wednesday));
            Assert.True(days.Has(DayOfWeek.Thursday));
            Assert.False(days.Has(DayOfWeek.Friday));
            Assert.True(days.Has(DayOfWeek.Saturday));
            Assert.True(days.Has(DayOfWeek.Sunday));

            var dayTest = new Set<DayOfWeek>(DayOfWeek.Sunday);
            days -= dayTest;
            Assert.False(days.Has(DayOfWeek.Monday));
            Assert.True(days.Has(DayOfWeek.Tuesday));
            Assert.True(days.Has(DayOfWeek.Wednesday));
            Assert.True(days.Has(DayOfWeek.Thursday));
            Assert.False(days.Has(DayOfWeek.Friday));
            Assert.True(days.Has(DayOfWeek.Saturday));
            Assert.False(days.Has(DayOfWeek.Sunday));
            days += dayTest;

            days -= DayOfWeek.Saturday;
            Assert.False(days.Has(DayOfWeek.Monday));
            Assert.True(days.Has(DayOfWeek.Tuesday));
            Assert.True(days.Has(DayOfWeek.Wednesday));
            Assert.True(days.Has(DayOfWeek.Thursday));
            Assert.False(days.Has(DayOfWeek.Friday));
            Assert.False(days.Has(DayOfWeek.Saturday));
            Assert.True(days.Has(DayOfWeek.Sunday));
            days += DayOfWeek.Saturday;
            days.Invert();

            var s = days.ToJson();
            var x = s.FromJson<Set<DayOfWeek>>();
            Assert.False(x.IsEmpty());

            Assert.True(x.Has(DayOfWeek.Monday));
            Assert.True(x.Has(DayOfWeek.Friday));
            Assert.True(x.Has(DayOfWeek.Monday, DayOfWeek.Friday));
            Assert.True(x.Is(DayOfWeek.Monday, DayOfWeek.Friday));

            Assert.False(x.Has(DayOfWeek.Monday, DayOfWeek.Friday, DayOfWeek.Saturday));
            Assert.False(x.Is(DayOfWeek.Monday, DayOfWeek.Friday, DayOfWeek.Saturday));

        }
    }
}
