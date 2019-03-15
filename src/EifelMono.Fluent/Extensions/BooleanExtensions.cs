namespace EifelMono.Fluent.Extensions
{
    public static class BooleanExtensions
    {
        public static bool ToBool(this string thisValue)
                => bool.Parse(thisValue);
        public static (bool Ok, bool Value) ToBoolSafe(this string thisValue)
            => bool.TryParse(thisValue, out var value) ? (true, value) : (false, default);
        public static bool ToBoolTry(this string thisValue, bool defaultValue = default)
            => bool.TryParse(thisValue, out var value) ? value : defaultValue;
    }
}
