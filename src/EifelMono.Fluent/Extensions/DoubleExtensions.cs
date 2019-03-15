using System;

namespace EifelMono.Fluent.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToDouble(this string thisValue)
                => double.Parse(thisValue);
        public static (bool Ok, double Value) ToDoubleSafe(this string thisValue)
            => double.TryParse(thisValue, out var value) ? (true, value) : (false, default);
        public static double ToDoubleTry(this string thisValue, double defaultValue = default)
            => double.TryParse(thisValue, out var value) ? value : defaultValue;

        public static double Abs(this double thisValue)
            => Math.Abs(thisValue);
        public static double Min(this double thisValue, double value)
            => Math.Min(thisValue, value);
        public static double Max(this double thisValue, double value)
            => Math.Max(thisValue, value);
        public static bool InRangeOffset(this double thisValue, double value, double offset)
            => thisValue.InRange(value - offset, value + offset);
    }
}
