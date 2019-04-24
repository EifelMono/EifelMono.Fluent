using System;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class TaskTests : XunitCore
    {
        public TaskTests(ITestOutputHelper output) : base(output) { }

        public async Task<DateTime> WorkAsync(TimeSpan timeSpan, bool failAfterDelay, CancellationToken cancellationToken = default)
        {
            await Task.Delay(timeSpan, cancellationToken);
            if (failAfterDelay)
                AssertSomethingWentWrong();
            return DateTime.Now;
        }

#pragma warning disable IDE0060
        public async Task<DateTime> WorkWithExceptionAsync(TimeSpan timeSpan, bool failAfterDelay, CancellationToken cancellationToken = default)
        {
            throw new NullReferenceException();
#pragma warning disable CS0162 // Unreachable code detected
            await Task.Delay(timeSpan, cancellationToken);
            if (failAfterDelay)
                AssertFail();
            return DateTime.Now;
#pragma warning restore CS0162 // Unreachable code detected
        }
#pragma warning restore IDE0060

        [Fact]
        public async void TestTaskNormal_NoCancel()
        {
            {
                try
                {
                    var timeStamp = await WorkAsync(TimeSpan.FromSeconds(1), false);
                    AssertOk();
                }
                catch (Exception)
                {
                    AssertSomethingWentWrong();
                }
            }
            {
                try
                {
                    var timeStamp = await WorkWithExceptionAsync(TimeSpan.FromSeconds(1), false);
                    AssertSomethingWentWrong();
                }
                catch (Exception)
                {
                    AssertOk();
                }
            }
        }

        [Fact]
        public async void TestTaskWithCancelation_Cancel()
        {
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
                    AssertSomethingWentWrong();
                }
                catch (Exception)
                {
                    AssertOk();
                }
            }
            {
                var cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    cts.Cancel();
                });
                try
                {
                    var timeStamp = await WorkWithExceptionAsync(TimeSpan.FromSeconds(5), true, cts.Token);
                    AssertSomethingWentWrong();
                }
                catch (Exception)
                {
                    AssertOk();
                }
            }
        }

        [Fact]
        public async void TestTaskWithExternalCancelation_Cancel()
        {
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
                    AssertSomethingWentWrong();
                }
                catch (Exception)
                {
                    AssertOk();
                }
            }
            {
                var cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    cts.Cancel();
                });
                try
                {
                    var timeStamp = await WorkWithExceptionAsync(TimeSpan.FromSeconds(5), true).WithCancellation(cts.Token);
                    AssertSomethingWentWrong();
                }
                catch (Exception)
                {
                    AssertOk();
                }
            }
        }

        [Fact]
        public async void TestTaskWithExternalCancelation_NoCancel()
        {
            {
                var cts = new CancellationTokenSource();
                try
                {
                    var timeStamp = await WorkAsync(TimeSpan.FromSeconds(1), false).WithCancellation(cts.Token);
                    AssertOk();
                }
                catch (Exception)
                {
                    AssertSomethingWentWrong();
                }
            }
            {
                var cts = new CancellationTokenSource();
                try
                {
                    var timeStamp = await WorkWithExceptionAsync(TimeSpan.FromSeconds(1), false).WithCancellation(cts.Token);
                    AssertSomethingWentWrong();
                }
                catch (Exception)
                {
                    AssertOk();
                }
            }
        }

        [Fact]
        public async void DemoThatTheOriginalTaskIsNotCanceled()
        {
            Task task = Task.Delay(TimeSpan.FromSeconds(5));
            try
            {
                await task.WithCancellation(TimeSpan.FromSeconds(1));
                AssertSomethingWentWrong();
            }
            catch (Exception)
            {
                Assert.False(task.IsCompleted);
                await Task.Delay(TimeSpan.FromSeconds(5));
                Assert.True(task.IsCompleted);
                AssertOk();
            }

        }
    }
}
