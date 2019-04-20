using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Flow;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.FlowTests
{
    public class ActionListTests : XunitCore
    {
        public ActionListTests(ITestOutputHelper output) : base(output) { }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void ListTests(int countMax)
        {
            object sync = new object();
            var count = 0;
            var list = new ActionList();
            for (int i = 0; i < countMax; i++)
                list.Add(() =>
                {
                    lock (sync)
                        count++;
                });
            for (int i = 0; i < countMax; i++)
                Assert.True(list.Contains(list[i]));
            Assert.Equal(countMax, list.Count);

            count = 0;
            {
                var (Calls, Errors) = list.Invoke();
                Assert.Equal(countMax, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax, count);
            }
            count = 0;
            {
                var (Calls, Errors) = list.InvokeParallel();
                Assert.Equal(countMax, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax, count);
            }

            var action = list.Add(() =>
            {
                lock (sync)
                    count += 2;
            });

            Assert.Equal(countMax + 1, list.Count);

            count = 0;
            {
                var (Calls, Errors) = list.Invoke();
                Assert.Equal(countMax + 1, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax + 1 + 1, count);
            }
            count = 0;
            {
                var (Calls, Errors) = list.InvokeParallel();
                Assert.Equal(countMax + 1, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax + 1 + 1, count);
            }

            list.Remove(action);
            Assert.Equal(countMax, list.Count);

            count = 0;

            {
                var (Calls, Errors) = list.Invoke();
                Assert.Equal(countMax, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax, count);
            }
            count = 0;
            {
                var (Calls, Errors) = list.InvokeParallel();
                Assert.Equal(countMax, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax, count);
            }

            list.Clear();
            Assert.Equal(0, list.Count);
        }

        [Theory]
        [InlineData(10)]
        public void ListTestsError(int countMax)
        {
            object sync = new object();
            var count = 0;
            var list = new ActionList();
            for (int i = 0; i < countMax; i++)
                list.Add(() =>
                {
                    lock (sync)
                        count++;
                    throw new Exception();
                });
            Assert.Equal(countMax, list.Count);

            count = 0;
            {
                var (Calls, Errors) = list.Invoke();
                Assert.Equal(countMax, Calls);
                Assert.Equal(countMax, Errors);
                Assert.Equal(countMax, count);
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        public void ListMessureTests(int countMax)
        {
            object sync = new object();
            var count = 0;
            var list = new ActionList();
            for (int i = 0; i < countMax; i++)
                list.Add(() =>
                {
                    Thread.Sleep(10);
                    lock (sync)
                        count++;
                });

            Assert.Equal(countMax, list.Count);

            count = 0;
            {
                var stopwatch = Stopwatch.StartNew();
                var (Calls, Errors) = list.Invoke();
                stopwatch.Stop();
                Assert.Equal(countMax, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax, count);
                WriteLine($"Invoke in {stopwatch.ElapsedMilliseconds} ms");
                Assert.Equal(countMax, count);
            }

            count = 0;
            {
                var stopwatch = Stopwatch.StartNew();
                var (Calls, Errors) = list.InvokeParallel();
                stopwatch.Stop();
                Assert.Equal(countMax, Calls);
                Assert.Equal(0, Errors);
                Assert.Equal(countMax, count);
                WriteLine($"Invoke in {stopwatch.ElapsedMilliseconds} ms");
                Assert.Equal(countMax, count);
            }
        }

        [Theory]
        [InlineData(1, "2", 3.0, true, DayOfWeek.Thursday)]
        [InlineData(21, null, 3.0134, false, DayOfWeek.Monday)]
        public void ActionListArgTest(int a1, string a2, double a3, bool a4, DayOfWeek a5)
        {
            {
                object sync = new object();
                var count = 0;
                new ActionList<int>().Add((arg1) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a1, arg1);
                }).Invoke(a1);
                new ActionList<string>().Add((arg1) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a2, arg1);
                }).Invoke(a2);
                new ActionList<double>().Add((arg1) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a3, arg1);
                }).Invoke(a3);
                new ActionList<bool>().Add((arg1) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a4, arg1);
                }).Invoke(a4);
                new ActionList<DayOfWeek>().Add((arg1) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a5, arg1);
                }).Invoke(a5);
                Assert.Equal(5, count);
            }
            {
                object sync = new object();
                var count = 0;

                var l1 = new ActionList<int>();
                var c1 = l1.Add((arg1) =>
                 {
                     lock (sync)
                         count++;
                     Assert.Equal(a1, arg1);
                 });
                for (int i = 0; i < l1.Count; i++)
                    Assert.True(l1.Contains(l1[i]));
                Assert.True(l1.Contains(c1));
                l1.Invoke(a1);
                l1.InvokeParallel(a1);

                var l2 = new ActionList<int, string>();
                var c2 = l2.Add((arg1, arg2) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a1, arg1);
                    Assert.Equal(a2, arg2);
                });
                for (int i = 0; i < l2.Count; i++)
                    Assert.True(l2.Contains(l2[i]));
                Assert.True(l2.Contains(c2));
                l2.Invoke(a1, a2);
                l2.InvokeParallel(a1, a2);

                var l3 = new ActionList<int, string, double>();
                var c3 = l3.Add((arg1, arg2, arg3) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a1, arg1);
                    Assert.Equal(a2, arg2);
                    Assert.Equal(a3, arg3);
                });
                for (int i = 0; i < l3.Count; i++)
                    Assert.True(l3.Contains(l3[i]));
                Assert.True(l3.Contains(c3));
                l3.Invoke(a1, a2, a3);
                l3.InvokeParallel(a1, a2, a3);

                var l4 = new ActionList<int, string, double, bool>();
                var c4 = l4.Add((arg1, arg2, arg3, arg4) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a1, arg1);
                    Assert.Equal(a2, arg2);
                    Assert.Equal(a3, arg3);
                    Assert.Equal(a4, arg4);
                });
                for (int i = 0; i < l4.Count; i++)
                    Assert.True(l4.Contains(l4[i]));
                Assert.True(l4.Contains(c4));
                l4.Invoke(a1, a2, a3, a4);
                l4.InvokeParallel(a1, a2, a3, a4);

                var l5 = new ActionList<int, string, double, bool, DayOfWeek>();
                var c5 = l5.Add((arg1, arg2, arg3, arg4, arg5) =>
                {
                    lock (sync)
                        count++;
                    Assert.Equal(a1, arg1);
                    Assert.Equal(a2, arg2);
                    Assert.Equal(a3, arg3);
                    Assert.Equal(a4, arg4);
                    Assert.Equal(a5, arg5);
                });
                for (int i = 0; i < l5.Count; i++)
                    Assert.True(l5.Contains(l5[i]));
                Assert.True(l5.Contains(c5));
                l5.Invoke(a1, a2, a3, a4, a5);
                l5.InvokeParallel(a1, a2, a3, a4, a5);

                Assert.Equal(10, count);

                l1.Remove(c1);
                Assert.False(l1.Contains(c1));
                l2.Remove(c2);
                Assert.False(l2.Contains(c2));
                l3.Remove(c3);
                Assert.False(l3.Contains(c3));
                l4.Remove(c4);
                Assert.False(l4.Contains(c4));
                l5.Remove(c5);
                Assert.False(l5.Contains(c5));

                l1.Invoke(a1);
                l1.InvokeParallel(a1);
                l2.Invoke(a1, a2);
                l2.InvokeParallel(a1, a2);
                l3.Invoke(a1, a2, a3);
                l3.InvokeParallel(a1, a2, a3);
                l4.Invoke(a1, a2, a3, a4);
                l4.InvokeParallel(a1, a2, a3, a4);
                l5.Invoke(a1, a2, a3, a4, a5);
                l5.InvokeParallel(a1, a2, a3, a4, a5);

                Assert.Equal(10, count);
            }
        }
    }
}
