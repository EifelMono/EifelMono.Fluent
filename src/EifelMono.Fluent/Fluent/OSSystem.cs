using System;

#pragma warning disable CS8509 // The switch expression does not handle all possible inputs (it is not exhaustive).
namespace EifelMono.Fluent
{
    public enum OSSystem
    {
        Windows,
        Linux,
        MacOS,
    }

    public static class OSSystemExentsions
    {
        public static bool IsWindows(this OSSystem thisValue)
            => thisValue == OSSystem.Windows;
        public static bool IsLinux(this OSSystem thisValue)
            => thisValue == OSSystem.Linux;
        public static bool IsMacOS(this OSSystem thisValue)
            => thisValue == OSSystem.MacOS;

        public static void On(this OSSystem thisValue, Action onWindows, Action onLinux, Action onMacOS)
             => thisValue.OnWindows(onWindows).OnLinux(onLinux).OnMacOS(onMacOS);

        public static TResult On<TResult>(this OSSystem thisValue, Func<TResult> onWindows, Func<TResult> onLinux, Func<TResult> onMacOS)
            => thisValue switch
            {
                OSSystem.Windows => onWindows(),
                OSSystem.Linux => onLinux(),
                OSSystem.MacOS => onMacOS()
            };

        public static TResult On<T1, TResult>(this OSSystem thisValue, T1 value1, Func<T1, TResult> onWindows, Func<T1, TResult> onLinux, Func<T1, TResult> onMacOS)
            => thisValue switch
            {
                OSSystem.Windows => onWindows(value1),
                OSSystem.Linux => onLinux(value1),
                OSSystem.MacOS => onMacOS(value1)
            };

        public static OSSystem OnWindows(this OSSystem thisValue, Action onWindows)
        {
            if (thisValue.IsWindows())
                onWindows?.Invoke();
            return thisValue;
        }
        public static OSSystem OnLinux(this OSSystem thisValue, Action onLinux)
        {
            if (thisValue.IsLinux())
                onLinux?.Invoke();
            return thisValue;
        }
        public static OSSystem OnMacOS(this OSSystem thisValue, Action onMacOS)
        {
            if (thisValue.IsMacOS())
                onMacOS?.Invoke();
            return thisValue;
        }
    }
}
