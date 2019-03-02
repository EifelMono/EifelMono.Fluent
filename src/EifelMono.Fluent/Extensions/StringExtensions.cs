using System.Collections.Generic;
using System.IO;

namespace EifelMono.Fluent.Extensions
{
    public static partial class StringExtensions
    {
        public static string ToJoinString(this List<string> thisValue, string joinChar)
            => string.Join(joinChar, thisValue);
    }
}
