using System;

namespace EifelMono.Fluent.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string thisValue) where T : Enum
            => fluent.Enum.Parse<T>(thisValue);
        public static (bool Ok, T Value) ToSafeEnum<T>(this string thisValue, bool ignoreCase = false, T defaultValue = default) where T : Enum
            => fluent.Enum.SafeParse<T>(thisValue, ignoreCase, defaultValue);
        public static T ToTryEnum<T>(this string thisValue, bool ignoreCase = false, T defaultValue = default) where T : Enum
            => fluent.Enum.TryParse<T>(thisValue, ignoreCase, defaultValue);
    }
}
