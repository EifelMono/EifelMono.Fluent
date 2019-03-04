using System;

namespace EifelMono.Fluent.Extensions
{
    public static partial class GenericExtensions
    {
        public static T Pipe<T>(this T thisValue, Action<T> action)
        {
            action(thisValue);
            return thisValue;
        }
        public static T Pipe<T>(this T thisValue, Func<T, T> action)
            => action(thisValue);

        public static Tout Pipe<Tin, Tout>(this Tin thisValue, Func<Tin, Tout> action)
            => action(thisValue);
    }
}
