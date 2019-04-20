using System;
using System.Collections.Generic;
using System.Linq;

namespace EifelMono.Fluent.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string thisValue) where T : Enum
            => fluent.Enum.Parse<T>(thisValue);
        public static (bool Ok, T Value) ToEnumSafe<T>(this string thisValue, bool ignoreCase = false, T defaultValue = default) where T : Enum
            => fluent.Enum.ParseSafe<T>(thisValue, ignoreCase, defaultValue);
        public static T ToEnumTry<T>(this string thisValue, bool ignoreCase = false, T defaultValue = default) where T : Enum
            => fluent.Enum.TryParse<T>(thisValue, ignoreCase, defaultValue);

#pragma warning disable IDE0060 // Remove unused parameter
        public static List<T> Values<T>(this T thisValue) where T : Enum
            => fluent.Enum.Values<T>().ToList();
        public static List<string> Names<T>(this T thisValue) where T : Enum
            => fluent.Enum.Names<T>().ToList();

        public static T Next<T>(this List<T> thisValues, T currentValue) where T : Enum
        {
            var index = thisValues.IndexOf(currentValue);
            if (index < thisValues.Count - 1)
                currentValue = thisValues[++index];
            return currentValue;
        }

        public static T Previous<T>(this List<T> thisValues, T currentValue) where T : Enum
        {
            var index = thisValues.IndexOf(currentValue);
            if (index > 0)
                currentValue = thisValues[--index];
            return currentValue;
        }

        public static T Next<T>(this T thisValue) where T : Enum
            => thisValue.Values().Next(thisValue);
        public static T Previous<T>(this T thisValue) where T : Enum
            => thisValue.Values().Previous(thisValue);
        public static T First<T>(this T thisValue) where T : Enum
            => thisValue.Values().First();
        public static T Last<T>(this T thisValue) where T : Enum
            => thisValue.Values().Last();
    }
}
