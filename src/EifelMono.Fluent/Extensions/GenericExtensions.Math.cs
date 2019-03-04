using System;

namespace EifelMono.Fluent.Extensions
{
    public static partial class GenericExtensions
    {
        public static bool InRange<T>(this T thisValue, T minValue, T maxValue) where T : IComparable
           => thisValue.CompareTo(minValue) >= 0 && thisValue.CompareTo(maxValue) <= 0;

        /// <summary>
        /// return a thisValue between the the minValue and maxValue
        /// if thisValue is less MinValue, thisValue will become minValue
        /// if thisValue is greater maxValue, this value will become maxValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static T Clamp<T>(this T thisValue, T minValue, T maxValue) where T : IComparable<T>
            => thisValue.CompareTo(minValue) < 0 ? minValue : thisValue.CompareTo(maxValue) > 0 ? maxValue : thisValue;

        public static T MinValue<T>(this T thisValue, T minValue) where T : IComparable<T>
            => thisValue.CompareTo(minValue) < 0 ? thisValue : minValue;

        public static T MaxValue<T>(this T thisValue, T maxValue) where T : IComparable<T>
            => thisValue.CompareTo(maxValue) > 0 ? thisValue : maxValue;
    }
}
