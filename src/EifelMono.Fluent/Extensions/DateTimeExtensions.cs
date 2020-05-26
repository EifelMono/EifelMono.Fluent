using System;
namespace EifelMono.Fluent.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfYear(this DateTime thisValue)
            => new DateTime(thisValue.Year, 1, 1);
        public static DateTime LastDayOfYear(this DateTime thisValue)
            => new DateTime(thisValue.Year, 12, DateTime.DaysInMonth(thisValue.Year, 12));

        public static DateTime FirstDayOfMonth(this DateTime thisValue)
            => new DateTime(thisValue.Year, thisValue.Month, 1);
        public static DateTime LastDayOfMonth(this DateTime thisValue)
            => new DateTime(thisValue.Year, thisValue.Month, DateTime.DaysInMonth(thisValue.Year, thisValue.Month));

        public static bool IsSameYear(this DateTime thisValue, DateTime dateTime)
            => thisValue.Year == dateTime.Year;
        public static bool IsSameYearAndMonth(this DateTime thisValue, DateTime dateTime)
            => thisValue.Year == dateTime.Year && thisValue.Month == dateTime.Month;
    }
}
