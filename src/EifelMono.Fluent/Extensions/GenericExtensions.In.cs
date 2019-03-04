using System.Collections.Generic;

namespace EifelMono.Fluent.Extensions
{
    public static partial class GenericExtensions
    {
        public static (bool Ok, int Index, T Value) SafeIn<T>(this T thisValue, IEnumerable<T> values)
        {
            int index = -1;
            foreach (var value in values)
            {
                index++;
                if (value.Equals(thisValue))
                    return (true, index, value);
            }
            return (false, -1, default);
        }
        public static bool In<T>(this T thisValue, IEnumerable<T> values)
            => thisValue.SafeIn(values).Ok;

        public static bool In<T>(this T thisValue, params T[] values)
            => thisValue.SafeIn(values as IEnumerable<T>).Ok;
    }
}
