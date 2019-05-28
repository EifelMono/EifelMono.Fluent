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
        [InlineData(new DayOfWeek[] { DayOfWeek.Wednesday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Friday })]
        [InlineData(new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Friday })]
        public async void WaitQueueValueTest(DayOfWeek[] waitValues)
        {
            var v = new WaitQueueValue<DayOfWeek>();
            _ = Task.Run(async () =>
            {
                foreach (var dayOfWeek in fluent.Enum.Values<DayOfWeek>())
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.AddData(dayOfWeek);
                }
            });
            var result = await v.WaitValuesAsync(waitValues, TimeSpan.FromSeconds(1));
            Assert.True(result.Ok);
        }
    }
}
