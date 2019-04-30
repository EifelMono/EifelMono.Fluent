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
    public class WaitValueTests : XunitCore
    {
        public WaitValueTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void FlowExtensionsTest()
        {
            var v = new WaitValue<DayOfWeek>(DayOfWeek.Monday);
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                v.Value = DayOfWeek.Friday;
            });
            await v.WaitValuesAsync(fluent.@params(DayOfWeek.Monday, DayOfWeek.Friday));

            v.Value = DayOfWeek.Sunday;
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                v.Value = DayOfWeek.Friday;
            });
            await v.WaitValueAsync(fluent.@params(DayOfWeek.Monday, DayOfWeek.Friday));
        }

        [Fact]
        public async void WaitValueTest()
        {
            var v = new WaitValue<DayOfWeek>(DayOfWeek.Sunday);
            _ = Task.Run(async () =>
            {
                foreach (var dayOfWeek in fluent.Enum.Values<DayOfWeek>())
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    v.Value = dayOfWeek;
                }
            });
            Assert.True(await v.WaitValueAsync(DayOfWeek.Monday, TimeSpan.FromSeconds(1)));
            Assert.True(await v.WaitValueAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, TimeSpan.FromSeconds(1)));
            Assert.True(await v.WaitValueAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, TimeSpan.FromSeconds(1)));
            Assert.True(await v.WaitValueAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, TimeSpan.FromSeconds(1)));
            Assert.True(await v.WaitValueAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, TimeSpan.FromSeconds(1)));
            Assert.True(await v.WaitValueAsync(fluent.Enum.Values<DayOfWeek>().ToArray(), TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public async void WaitValuesTest()
        {
            for (var index = 0; index < 6; index++)
            {
                var v = new WaitValue<DayOfWeek>(DayOfWeek.Sunday);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    foreach (var dayOfWeek in fluent.Enum.Values<DayOfWeek>())
                        v.Value = dayOfWeek;
                });
                switch (index)
                {
                    case 0:
                        Assert.True(await v.WaitValuesAsync(DayOfWeek.Monday, TimeSpan.FromSeconds(1)));
                        break;
                    case 1:
                        Assert.True(await v.WaitValuesAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, TimeSpan.FromSeconds(1)));
                        break;
                    case 2:
                        Assert.True(await v.WaitValuesAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, TimeSpan.FromSeconds(1)));
                        break;
                    case 3:
                        Assert.True(await v.WaitValuesAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, TimeSpan.FromSeconds(1)));
                        break;
                    case 4:
                        Assert.True(await v.WaitValuesAsync(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, TimeSpan.FromSeconds(1)));
                        break;
                    case 5:
                        Assert.True(await v.WaitValuesAsync(fluent.Enum.Values<DayOfWeek>().ToArray(), TimeSpan.FromSeconds(1)));
                        break;
                }
            }
        }
        [Fact]
        public async void WaitValueDocTest()
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
                if (v == DayOfWeek.Monday)
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

        [DataContract]
        private enum TestEnums
        {
            [DataMember]
            A,
            [DataMember]
            B,
            [DataMember]
            C,
            [DataMember]
            D,
            [DataMember]
            AA,
            [DataMember]
            BB,
            [DataMember]
            CC,
            [DataMember]
            DD
        }

        [Fact]
        public void WaitValueOperatorTest()
        {
            var v = new WaitValue<DayOfWeek>(DayOfWeek.Monday);
            {
                var x = v;
                Assert.Equal(DayOfWeek.Monday, x.Value);
            }
            {
#pragma warning disable IDE0007 // Use implicit type
                WaitValue<DayOfWeek> x = v;
#pragma warning restore IDE0007 // Use implicit type
                Assert.Equal(DayOfWeek.Monday, x.Value);
            }
            {
                DayOfWeek x = v;
                Assert.Equal(DayOfWeek.Monday, x);
            }

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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(10)]
        [InlineData(33)]
        [InlineData(55)]
        [InlineData(77)]
        [InlineData(100)]
        [InlineData(333)]
        [InlineData(777)]
        [InlineData(555)]
        [InlineData(1000)]
        public async void WaitValueSingleDataTest(int msecWaitOnStart)
        {
            var dayOfWeek = new WaitEnumValue<DayOfWeek>(DayOfWeek.Sunday);

            WriteLine($"start dayofweek {dayOfWeek.Value}");

            var dayOfWeekMonday = false;
            _ = Task.Run(async () =>
            {
                await dayOfWeek.WaitValueAsync(DayOfWeek.Monday);
                WriteLine($"wait ready Monday {dayOfWeek.Value}");
                dayOfWeekMonday = true;
                dayOfWeek.Value = DayOfWeek.Wednesday;
            });

            var dayOfWeekWednesday = false;
            _ = Task.Run(async () =>
            {
                await dayOfWeek.WaitValueAsync(DayOfWeek.Wednesday);
                WriteLine($"wait ready Wednesday {dayOfWeek.Value}");
                dayOfWeekWednesday = true;
                dayOfWeek.Value = DayOfWeek.Friday;
            });


            if (msecWaitOnStart > 0)
                await Task.Delay(TimeSpan.FromMilliseconds(msecWaitOnStart));
            dayOfWeek.Value = DayOfWeek.Monday;

            await dayOfWeek.WaitValueAsync(DayOfWeek.Friday);
            WriteLine($"wait ready Friday {dayOfWeek.Value}");

            Assert.True(dayOfWeekMonday);
            Assert.True(dayOfWeekWednesday);
        }

        [Theory]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday },
            new DayOfWeek[] { DayOfWeek.Monday },
            true)]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday },
            new DayOfWeek[] { },
            false)]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            true)]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday },
            new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            false)]
        public async void WaitValuesDataTest(DayOfWeek[] dayOfWeeksSend, DayOfWeek[] dayOfWeeksWait, bool result)
        {
            var test = new WaitEnumValue<DayOfWeek>(DayOfWeek.Sunday);

            WriteLine($"start dayofweek {test.Value}");

            var waitTask = test.WaitValuesAsync(dayOfWeeksWait, TimeSpan.FromSeconds(1).AsToken());
            var send = 0;
            foreach (var value in dayOfWeeksSend)
                _ = Task.Run(() =>
                {
                    WriteLine($"Set {value}");
                    send++;
                    test.Value = value;
                });
            Assert.Equal(result, await waitTask);
            if (result)
                Assert.Equal(dayOfWeeksSend.Length, send);
        }

        [Theory]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday },
           new DayOfWeek[] { DayOfWeek.Monday },
           true)]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday },
           new DayOfWeek[] { },
           false)]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
           new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
           true)]
        [InlineData(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday },
           new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
           true)]
        public async void WaitValueDataTest(DayOfWeek[] dayOfWeeksSend, DayOfWeek[] dayOfWeeksWait, bool result)
        {
            var test = new WaitEnumValue<DayOfWeek>(DayOfWeek.Sunday);

            WriteLine($"start dayofweek {test.Value}");

            var waitTask = test.WaitValueAsync(dayOfWeeksWait, TimeSpan.FromSeconds(1).AsToken());
            var send = 0;
            foreach (var value in dayOfWeeksSend)
                _ = Task.Run(() =>
                {
                    send++;
                    test.Value = value;
                });
            Assert.Equal(result, await waitTask);
            if (result)
                Assert.Equal(dayOfWeeksSend.Length, send);
        }
    }
}
