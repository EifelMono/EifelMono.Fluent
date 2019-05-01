using System.Collections.Generic;

namespace EifelMono.Fluent.Extensions
{
    public static partial class GenericExtensions
    {
        public static (bool Ok, int Index, T Value) IsSafe<T>(this T thisValue, IEnumerable<T> values)
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
        public static bool Is<T>(this T thisValue, IEnumerable<T> values)
            => thisValue.IsSafe(values).Ok;

        public static bool Is<T>(this T thisValue, params T[] values)
            => thisValue.IsSafe(values as IEnumerable<T>).Ok;
    }
}
