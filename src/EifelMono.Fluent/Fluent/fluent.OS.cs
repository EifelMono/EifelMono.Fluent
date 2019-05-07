using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EifelMono.Fluent.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class OS
        {
            public static OSSystem System
            {
                get
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        return OSSystem.Windows;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        return OSSystem.Linux;
                    return OSSystem.MacOS;
                }
            }

            public static void On(Action onWindows, Action onLinux, Action onMacOS)
                => OS.System.OnWindows(onWindows).OnLinux(onLinux).OnMacOS(onMacOS);

            public static OSSystem OnWindows(Action onWindows)
            {
                if (OS.System.IsWindows())
                    onWindows?.Invoke();
                return OS.System;
            }
            public static OSSystem OnLinux(Action onLinux)
            {
                if (OS.System.IsLinux())
                    onLinux?.Invoke();
                return OS.System;
            }
            public static OSSystem OnMacOS(Action onMacOS)
            {
                if (OS.System.IsMacOS())
                    onMacOS?.Invoke();
                return OS.System;
            }

#if !NETSTANDARD1_6
            public static void OpenUrl(string url)
                 => OS.System
                    .OnWindows(() => Process.Start(new ProcessStartInfo("cmd", $"/C start {url.Replace("&", "^&")}") { CreateNoWindow = true }))
                    .OnLinux(() => Process.Start("xdg-open", url))
                    .OnMacOS(() => Process.Start("open", url));
#endif
        }
    }
}
