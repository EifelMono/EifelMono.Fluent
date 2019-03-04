using System;

namespace EifelMono.Fluent.Extensions
{
    public static class IntExtensions
    {
        public static int ToInt(this string thisValue)
            => int.Parse(thisValue);
        public static (bool Ok, int Value) ToSafeInt(this string thisValue)
            => int.TryParse(thisValue, out var value) ? (true, value) : (false, default);
        public static int ToTryInt(this string thisValue, int defaultValue = default)
            => int.TryParse(thisValue, out var value) ? value : defaultValue;

        public static int Abs(this int thisValue)
            => Math.Abs(thisValue);
        public static int Min(this int thisValue, int value)
            => Math.Min(thisValue, value);
        public static int Max(this int thisValue, int value)
            => Math.Max(thisValue, value);
        public static bool InRangeOffset(this int thisValue, int value, int offset)
            => thisValue.InRange(value - offset, value + offset);
    }
}
