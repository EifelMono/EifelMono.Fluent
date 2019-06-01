using System;
using System.Collections.Generic;
using System.Linq;

namespace EifelMono.Fluent.Extensions
{
    public static partial class StringExtensions
    {
        public static bool IsNullOrEmpty(this string thisValue)
            => string.IsNullOrEmpty(thisValue);
        public static bool IsNotNullOrEmpty(this string thisValue)
            => !thisValue.IsNullOrEmpty();
        public static bool IsLengthGreater(this string thisValue, int value)
            => thisValue is null ? false : thisValue.Length > value;

        public static string ToJoinString<T>(this IEnumerable<T> thisValue, string joinChar)
            => string.Join(joinChar, thisValue);
        public static string Repeat(this string thisValue, int count)
            => string.Concat(Enumerable.Repeat(thisValue, count));
        public static string NewLine(this string thisValue)
            => string.Concat(thisValue, Environment.NewLine);
        public static string IfEndsWithRemove(this string thisValue, string text)
            => thisValue.EndsWith(text)
                ? thisValue.Substring(0, thisValue.Length - text.Length)
                : thisValue;
        public static bool StartsWithDigit(this string thisValue)
            => thisValue.IsLengthGreater(1) ? thisValue[0].IsDigit() : false;

        public static bool AreDigits(this string thisValue)
            => thisValue.All(char.IsDigit);
    }
}
