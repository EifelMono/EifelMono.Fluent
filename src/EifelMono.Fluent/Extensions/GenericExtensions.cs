using System;

namespace EifelMono.Fluent.Extensions
{
    public static partial class GenericExtensions
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static T Backing<T>(this object thisValue, ref T backing, Func<T> action, T defaultValue = default)
            => Equals(backing, defaultValue) ? backing = action.Invoke() : backing;
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
