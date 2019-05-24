using System;

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
