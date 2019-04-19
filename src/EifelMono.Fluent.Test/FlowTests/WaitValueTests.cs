using System;
using System.Threading.Tasks;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class WaitValueTests : XunitCore
    {
        public WaitValueTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void WaitValueIntTest()
        {
            var v = new WaitValue<int>();
            {
                v.Value = 0;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    // v.Value = 1;
                });
                var result = await v.WaitValueAsync(0);
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = 0;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.Value = 1;
                });
                var result = await v.WaitValueAsync(1);
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = 0;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    // v.Value = 1;
                });
                var result = await v.WaitValueAsync(0, TimeSpan.FromMilliseconds(100));
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = 0;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                    v.Value = 1;
                });
                var result = await v.WaitValueAsync(1, TimeSpan.FromMilliseconds(100));
                WriteLine($"{result} {v.Value}");
                Assert.False(result);
            }
        }

        private enum TestEnums
        {
            A,
            B,
            C,
            D,
            AA,
            BB,
            CC,
            DD

        }
        [Fact]
        public async void WaitValueEnumTest()
        {
            var v = new WaitEnumValue<TestEnums>();
            {
                v.Value = TestEnums.A;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    // v.Value = TestEnums.B;
                });
                var result = await v.WaitValueAsync(TestEnums.A);
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = TestEnums.A;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.Value = TestEnums.B;
                });
                var result = await v.WaitValueAsync(TestEnums.B);
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = TestEnums.A;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    // v.Value = TestEnums.B;
                });
                var result = await v.WaitValueAsync(TestEnums.A, TimeSpan.FromMilliseconds(100));
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = TestEnums.A;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                    v.Value = TestEnums.B;
                });
                var result = await v.WaitValueAsync(TestEnums.B, TimeSpan.FromMilliseconds(100));
                WriteLine($"{result} {v.Value}");
                Assert.False(result);
            }
        }

        [Fact]
        public async void WaitValueEnumNextTest()
        {
            var v = new WaitEnumValue<TestEnums>();
            {
                v.Value = TestEnums.A;

                var b = v.Next();
                Assert.Equal(TestEnums.B, b);
                Assert.Equal(TestEnums.B, v.Value);
            }

            {
                v.Value = TestEnums.B;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.Next();
                });
                var result = await v.WaitValueAsync(TestEnums.C, TimeSpan.FromMilliseconds(500));
                WriteLine($"{result} {v.Value}");
                Assert.True(result);
            }

            {
                v.Value = TestEnums.B;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    v.Next();
                });
                var result = await v.WaitValueAsync(TestEnums.C, TimeSpan.FromMilliseconds(100));
                WriteLine($"{result} {v.Value}");
                Assert.False(result);
            }
        }
    }
}
