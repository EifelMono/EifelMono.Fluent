using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> thisValue, Action<T> action)
        {
            foreach (var value in thisValue)
                action?.Invoke(value);
            return thisValue;
        }
        public static IEnumerable<T> ForEachIndex<T>(this IEnumerable<T> thisValue, Action<T, int> action)
        {
            var index = 0;
            foreach (var value in thisValue)
            {
                action?.Invoke(value, index);
                index++;
            }
            return thisValue;
        }
    }
}
