using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Extensions
{
    public static class BooleanExtensions
    {
        public static bool ToBool(this string thisValue)
                => bool.Parse(thisValue);
        public static (bool Ok, bool Value) ToSafeBool(this string thisValue)
            => bool.TryParse(thisValue, out var value) ? (true, value) : (false, default);
        public static bool ToTryBool(this string thisValue, bool defaultValue = default)
            => bool.TryParse(thisValue, out var value) ? value : defaultValue;
    }
}
