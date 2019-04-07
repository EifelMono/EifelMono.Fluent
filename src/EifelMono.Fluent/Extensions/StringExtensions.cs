using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EifelMono.Fluent.Extensions
{
    public static partial class StringExtensions
    {
        public static string ToJoinString(this List<string> thisValue, string joinChar)
            => string.Join(joinChar, thisValue);
        public static string Repeat(this string thisValue, int count)
            => string.Concat(Enumerable.Repeat(thisValue, count));
        public static string NewLine(this string thisValue)
            => string.Concat(thisValue, Environment.NewLine);
        public static string IfEndsWithRemove(this string thisValue, string text)
            => thisValue.EndsWith(text)
                ? thisValue.Substring(0, thisValue.Length - text.Length)
                : thisValue;
        public static bool FirstCharIsNumber(this string thisValue)
            => Regex.IsMatch(thisValue ?? "", @"^\d");
    }
}
