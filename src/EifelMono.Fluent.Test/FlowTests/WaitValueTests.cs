using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;
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
        public async void WaitValueDocText()
        {
            {
                var v = new WaitValue<int>(0);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.Value = 1;
                });
                if (await v.WaitValueAsync(1, TimeSpan.FromSeconds(1)) is var result && result)
                    Debug.WriteLine("Value Changed to 1");
                else
                    Debug.WriteLine("TimeOut");
            }
            {
var v = new WaitEnumValue<DayOfWeek>(DayOfWeek.Friday);
_ = Task.Run(async () =>
{
    await Task.Delay(TimeSpan.FromMilliseconds(100));
    v.Value = DayOfWeek.Sunday;
});
if (await v.WaitValueAsync(DayOfWeek.Sunday, TimeSpan.FromSeconds(1)) is var result && result)
    Debug.WriteLine("Value Changed to DayOfWeek.Sunday");
else
    Debug.WriteLine("TimeOut");
v.Next();
if (v== DayOfWeek.Monday)
    Debug.WriteLine("Next is Ok");
else
    Debug.WriteLine("Next error!");
v.Previous();
if (v == DayOfWeek.Sunday)
    Debug.WriteLine("Previous is Ok");
else
    Debug.WriteLine("Previous error!");
v.Last();
if (v == DayOfWeek.Saturday)
    Debug.WriteLine("Last is Ok");
else
    Debug.WriteLine("Last error!");
v.First();
if (v == DayOfWeek.Sunday)
    Debug.WriteLine("First is Ok");
else
    Debug.WriteLine("First error!");
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
        public void WaitValueJsonTest()
        {
            {
                var v = new WaitValue<int>();
                WriteLine(v.ToJson());
            }

            {
                var v = new WaitValue<int>(1);
                WriteLine(v.ToJson());
            }

            {
                var v = new WaitEnumValue<TestEnums>();
                WriteLine(v.ToJson());
            }
            {
                var v = new WaitEnumValue<TestEnums>(TestEnums.B);
                WriteLine(v.ToJson());
            }
        }

        [Fact]
        public async void WaitValueOperatorTest()
        {
            {
                WaitValue<int> v = 1;
                Assert.Equal(1, v.Value);
                int v1 = v;
                Assert.Equal(1, v1);

                v = 4711;
                Assert.Equal(4711, v.Value);
                int v2 = v;
                Assert.Equal(4711, v2);
            }

            {
                WaitValue<TestEnums> v = TestEnums.A;
                Assert.Equal(TestEnums.A, v.Value);
                TestEnums v1 = v;
                Assert.Equal(TestEnums.A, v1);

                v = TestEnums.D;
                Assert.Equal(TestEnums.D, v.Value);
                TestEnums v2 = v;
                Assert.Equal(TestEnums.D, v2);
            }

            {
                WaitValue<int> v = 1;
                Assert.Equal(1, v.Value);

                v = 4711;
                Assert.Equal(4711, v.Value);
            }
            {
                var v = new WaitValue<int>(1);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.Value = 1;
                });

                int x = v;
                Assert.Equal(x, v.Value);

                await v.WaitValueAsync(1);
            }
        }

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
                Assert.Equal(TestEnums.A, v.Value);

                var b = v.Next();
                Assert.Equal(TestEnums.B, b);
                Assert.Equal(TestEnums.B, v.Value);

                var c = v.Previous();
                Assert.Equal(TestEnums.A, c);
                Assert.Equal(TestEnums.A, v.Value);

                var d = v.Previous();
                Assert.Equal(TestEnums.A, d);
                Assert.Equal(TestEnums.A, v.Value);

                var e = v.Last();
                Assert.Equal(TestEnums.DD, e);
                Assert.Equal(TestEnums.DD, v.Value);

                var f = v.Next();
                Assert.Equal(TestEnums.DD, f);
                Assert.Equal(TestEnums.DD, v.Value);

                var g = v.First();
                Assert.Equal(TestEnums.A, g);
                Assert.Equal(TestEnums.A, v.Value);
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
