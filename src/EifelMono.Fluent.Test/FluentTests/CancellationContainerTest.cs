using System;
using System.Threading.Tasks;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FluentTests
{
    public class CancellationContainerTest : XunitCore
    {

        public CancellationContainerTest(ITestOutputHelper output) : base(output) { }


        [Fact]
        public async void Cointainer_WithTimeOut()
        {
            using var container = new CancellationContainer();
            container.AddSource("cts1");
            container.AddTimeOut(TimeSpan.FromSeconds(2), "timeout");

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5), container.Token);
                if (!container["timeout"].Token.IsCancellationRequested)
                    container["cts1"].Source.Cancel();
            }, container.Token);

            try
            {
                await Task.Delay(-1, container.Token);
                AssertSomethingWentWrong();
            }
            catch (Exception ex)
            {
                Assert.True(ex is OperationCanceledException);
            }
            Assert.True(container["timeout"].Token.IsCancellationRequested);
        }


        [Fact]
        public async void Cointainer_WithSecondsTimeOut()
        {
            using var container = new CancellationContainer();
            container.AddSource("cts1");
            container.AddSecondsTimeOut(2, "timeout");
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5), container.Token);
                if (!container["timeout"].Token.IsCancellationRequested)
                    container["cts1"].Source.Cancel();
            }, container.Token);
            try
            {
                await Task.Delay(-1, container.Token);
                AssertSomethingWentWrong();
            }
            catch (Exception ex)
            {
                Assert.True(ex is OperationCanceledException);
            }
            Assert.True(container["timeout"].Token.IsCancellationRequested);
        }
    }
}
