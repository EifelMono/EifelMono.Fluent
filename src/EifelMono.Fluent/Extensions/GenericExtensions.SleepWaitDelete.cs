using System;
using System.Threading;
using System.Threading.Tasks;

namespace EifelMono.Fluent.Extensions
{
    public static partial class GenericExtensions
    {
#if (!NETSTANDARD1_6)
        public static T Sleep<T>(this T thisValue, TimeSpan timespane)
        {
            Thread.Sleep(timespane);
            return thisValue;
        }

        public static T Sleep<T>(this T thisValue, int msec)
        {
            Thread.Sleep(msec);
            return thisValue;
        }
#endif
        public static async Task<T> WaitAsync<T>(this T thisValue, TimeSpan timespane)
        {
            await Task.Delay(timespane).ConfigureAwait(false);
            return thisValue;
        }
        public static async Task<T> WaitAsync<T>(this T thisValue, int msec)
        {
            await Task.Delay(msec).ConfigureAwait(false);
            return thisValue;
        }
    }

}
