using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class CancellationContainerTest : XunitCore
    {
        public CancellationContainerTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void Test_DisposeAfterAction()
        {
            var vcs1 = new CancellationTokenSource();
            var container = vcs1.NewContainer("vcs1");
            container.AddSource("cs1");
            container.AddSource("cs2");
            container.AddSecondsTimeOut(1, "timeout1");
            try
            {
                await CancellationContainer.Delay(-1, container);
            }
            catch (Exception ex)
            {
                Assert.True(ex is OperationCanceledException);
            }
            Assert.Empty(container.Items);
        }

        [Fact]
        public async void Test_NotDisposeAfterAction()
        {
            var vcs1 = new CancellationTokenSource();
            var container = vcs1.NewContainer("vcs1")
                .SetDiposeAfterAction(false);
            container.AddSource("cs1");
            container.AddSource("cs2");
            container.AddSecondsTimeOut(1, "timeout1");
            try
            {
                await CancellationContainer.Delay(-1, container);
            }
            catch (Exception ex)
            {
                Assert.True(ex is OperationCanceledException);
            }
            Assert.Single(container.CanceledItems);
            Assert.NotEmpty(container.Items);

            Assert.True(container["timeout1"].Token.IsCancellationRequested);

            container.Dispose();
        }


        [Fact]
        public async void Test_NotDisposeAfterActionMore()
        {
            var vcs1 = new CancellationTokenSource();
            var container = vcs1.NewContainer("vcs1")
                .SetDiposeAfterAction(false);
            container.AddSource("cs1");
            container.AddSource("cs2");
            container.AddSecondsTimeOut(1, "timeout1");
            try
            {
                await CancellationContainer.Delay(-1, container);
            }
            catch (Exception ex)
            {
                Assert.True(ex is OperationCanceledException);
            }
            container["cs1"].Source.Cancel();
            container["cs2"].Source.Cancel();

            Assert.Equal(3, container.CanceledItems.Count);
            Assert.NotEmpty(container.Items);

            Assert.True(container["timeout1"].Token.IsCancellationRequested);

            container.Dispose();
        }
    }

}
