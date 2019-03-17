using System;

namespace EifelMono.Fluent.Extensions
{
    public static class FloatExtensions
    {
        public static float ToFloat(this string thisValue)
                => float.Parse(thisValue);
        public static (bool Ok, float Value) ToFloatSafe(this string thisValue)
            => float.TryParse(thisValue, out var value) ? (true, value) : (false, default);
        public static float ToTryFloat(this string thisValue, float defaultValue = default)
            => float.TryParse(thisValue, out var value) ? value : defaultValue;

        public static float Abs(this float thisValue)
            => Math.Abs(thisValue);
        public static bool InRangeOffset(this float thisValue, float value, float offset)
            => thisValue.InRange(value - offset, value + offset);
    }
}
