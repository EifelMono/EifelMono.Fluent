using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class ConcurrentExtendedQueueTests : XunitCore
    {
        public ConcurrentExtendedQueueTests(ITestOutputHelper output) : base(output) { }

        [Theory]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(20)]
        public void Test_FillAndClear(int maxCount)
        {
            var queue = new ConcurrentExtendedQueue<string>();
            Assert.Empty(queue);
            for (int i = 0; i < maxCount; i++)
                queue.Enqueue(i.ToString());
            Assert.Equal(maxCount, queue.Count);
            queue.Clear();
            Assert.Empty(queue);
        }


        [Theory]
        [InlineData(100, 10)]
        [InlineData(100, 20)]
        public void Test_FillAndClearMin(int maxCount, int minCount)
        {
            var queue = new ConcurrentExtendedQueue<string>();
            Assert.Empty(queue);
            for (int i = 0; i < maxCount; i++)
                queue.Enqueue(i.ToString());
            Assert.Equal(maxCount, queue.Count);

            queue.ClearUntil(minCount);
            Assert.Equal(minCount, queue.Count);
        }
    }
}
