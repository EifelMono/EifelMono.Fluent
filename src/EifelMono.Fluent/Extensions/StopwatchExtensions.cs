using System.Diagnostics;

namespace EifelMono.Fluent.Extensions
{
    public static class StopwatchExtensions
    {
        public static bool ElapsedSeconds(this Stopwatch thisValue, int seconds)
            => thisValue.ElapsedMilliseconds / 1000 >= seconds;
    }
}
