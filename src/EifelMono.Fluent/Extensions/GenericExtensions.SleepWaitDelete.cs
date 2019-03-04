using System;
using System.Threading;

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
    }

}
