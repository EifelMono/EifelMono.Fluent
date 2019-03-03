using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EifelMono.Fluent.Extensions
{
    public static partial class StringExtensions
    {
        public static string ToJoinString(this List<string> thisValue, string joinChar)
            => string.Join(joinChar, thisValue);
        public static string Repeat(this string thisValue, int count)
            => string.Concat(Enumerable.Repeat(thisValue, count));
        public static string Repeat(this string thisValue, string repeatString, int count)
            => string.Concat(thisValue, string.Concat(Enumerable.Repeat(repeatString, count)));
        public static string NewLine(this string thisValue)
            => string.Concat(thisValue, Environment.NewLine);
    }
}
