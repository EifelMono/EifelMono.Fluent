using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class WaitQueueValueTests : XunitCore
    {
        public WaitQueueValueTests(ITestOutputHelper output) : base(output) { }

        [Theory]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Tuesday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Wednesday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Thursday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Saturday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Sunday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday,
                DayOfWeek.Saturday, DayOfWeek.Sunday})]
        public async void WaitQueueValue1Test(DayOfWeek[] waitValues)
        {
            for (int index = 0; index < 5; index++)
            {
                var v = new WaitQueueValue<DayOfWeek>();
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    foreach (var dayOfWeek in fluent.Enum.Values<DayOfWeek>())
                    {
                        v.AddData(dayOfWeek);
                    }
                });
                var result = await v.WaitValuesAsync(waitValues, TimeSpan.FromSeconds(1));
                Assert.True(result.Ok);
            }
        }
        [Theory]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Tuesday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Wednesday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Thursday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Saturday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Sunday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday,
            DayOfWeek.Saturday, DayOfWeek.Sunday})]
        public async void WaitQueueValue2Test(DayOfWeek[] waitValues)
        {
            for (int index = 0; index < 5; index++)
            {
                var v = new WaitQueueValue<DayOfWeek>();
                var task = v.WaitValuesAsync(waitValues, TimeSpan.FromSeconds(1));
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    foreach (var dayOfWeek in fluent.Enum.Values<DayOfWeek>())
                    {
                        v.AddData(dayOfWeek);
                    }
                });
                var result = await task;
                Assert.True(result.Ok);
            }
        }
    }
}
