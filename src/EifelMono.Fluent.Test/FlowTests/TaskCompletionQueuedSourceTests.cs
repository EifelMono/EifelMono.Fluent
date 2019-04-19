using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class TaskCompletionQueuedSourceTests : XunitCore
    {
        public TaskCompletionQueuedSourceTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void SingleTest()
        {
            var tq = new TaskCompletionQueuedSource<string>();

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                tq.NewData("Hallo");
            });

            if (await tq.WaitValueAsync() is var result && result.Ok)
                Assert.Equal("Hallo", result.Value);
            else
                AssertSomethingWentWrong();
        }


        [Fact]
        public async void SingleTest_WithCancel()
        {
            var tq = new TaskCompletionQueuedSource<string>();

            var cancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                tq.NewData("Hallo");
            });

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                cancellationTokenSource.Cancel();
            });


            if (await tq.WaitValueAsync(cancellationTokenSource.Token) is var result && result.Ok)
                AssertSomethingWentWrong();
            else
            {
                Assert.False(result.Ok);
                Assert.NotNull(result.Exception);
            }
        }


        [Fact]
        public async void MultiTest()
        {
            var tq = new TaskCompletionQueuedSource<string>();

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                for (int i = 0; i < 5; i++)
                {
                    var sendText = $"Hallo{i}";
                    WriteLine($"sendText={sendText}");
                    tq.NewData(sendText);
                    await Task.Delay(TimeSpan.FromMilliseconds(i * 10));
                }
            });

            var waitData = new List<string>();
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (await tq.WaitValueAsync() is var result && result.Ok)
                {
                    var receiveTextExpected = $"Hallo{waitData.Count}";
                    WriteLine($"receiveText={result.Value} receiveTextExpected={receiveTextExpected}");
                    Assert.Equal(receiveTextExpected, result.Value);
                    waitData.Add(result.Value);
                    await Task.Delay(TimeSpan.FromMilliseconds(waitData.Count * 200));
                }
                else
                    AssertSomethingWentWrong();
                if (waitData.Count == 5)
                    break;
                if (stopwatch.ElapsedSeconds(5))
                    AssertSomethingWentWrong();
            }
            Assert.Equal(5, waitData.Count);
        }



        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(9)]

        public async void SingleTest_WithCancels_FalseTest(int index)
        {
            var tq = new TaskCompletionQueuedSource<string>();

            var cancellationTokenSources = new List<CancellationTokenSource>();
            for (int i = 0; i < 10; i++)
                cancellationTokenSources.Add(new CancellationTokenSource());

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                tq.NewData("Hello");
            });

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                cancellationTokenSources[index].Cancel();
            });


            if (await tq.WaitValueAsync(cancellationTokenSources.Select(cs => cs.Token).ToArray()) is var result && result.Ok)
                AssertSomethingWentWrong();
            else
            {
                Assert.False(result.Ok);
                Assert.NotNull(result.Exception);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(9)]
        public async void SingleTest_WithCancels_TrueTest(int index)
        {
            var tq = new TaskCompletionQueuedSource<string>();

            var cancellationTokenSources = new List<CancellationTokenSource>();
            for (int i = 0; i < 10; i++)
                cancellationTokenSources.Add(new CancellationTokenSource());

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                tq.NewData("Hello");
            });

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                cancellationTokenSources[index].Cancel();
            });


            if (await tq.WaitValueAsync(cancellationTokenSources.Select(cs => cs.Token).ToArray())
                is var result && result.Ok && result.Value == "Hello")
                Assert.True(result.Ok);
            else
                AssertSomethingWentWrong();
        }
    }
}
