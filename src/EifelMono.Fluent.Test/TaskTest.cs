using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Test
{
    public class TaskTest : XunitCore
    {
        public TaskTest(ITestOutputHelper output) : base(output) { }

        public async Task<DateTime> WorkAsync(TimeSpan timeSpan, bool withCancel, CancellationToken cancellationToken = default)
        {
            await Task.Delay(timeSpan, cancellationToken);
            if (withCancel)
                Fail("xx");
            return DateTime.Now;
        }

        [Fact]
        public async void TestTaskNormal_NoCancel()
        {
            try
            {
                var timeStamp = await WorkAsync(TimeSpan.FromSeconds(1), false);
                Assert.True(true);
            }
            catch (Exception)
            {
                Fail("Something went wrong");
                Assert.True(true);
            }
        }

        [Fact]
        public async void TestTaskWithCancelation_Cancel()
        {
            var cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                cts.Cancel();
            });
            try
            {
                var timeStamp = await WorkAsync(TimeSpan.FromSeconds(5), true, cts.Token);
                Fail("Something went wrong");
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async void TestTaskWithExternalCancelation_Cancel()
        {
            var cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                cts.Cancel();
            });
            try
            {
                var timeStamp = await WorkAsync(TimeSpan.FromSeconds(5), true).WithCancellation(cts.Token);
                Fail("Something went wrong");
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async void TestTaskWithExternalCancelation_NoCancel()
        {
            var cts = new CancellationTokenSource();
            try
            {
                var timeStamp = await WorkAsync(TimeSpan.FromSeconds(1), false).WithCancellation(cts.Token);
                Assert.True(true);
            }
            catch (Exception)
            {
                Fail("Something went wrong");

            }
        }
    }
}
