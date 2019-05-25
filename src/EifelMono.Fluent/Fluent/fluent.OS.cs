using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8509 // The switch expression does not handle all possible inputs (it is not exhaustive).
namespace EifelMono.Fluent
{
    public static partial class fluent
    {
        public static class OS
        {
            public static OSSystem? _System = null;
            public static OSSystem System
            {
                get
                {
                    if (_System is object)
                        return (OSSystem)_System;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        return (OSSystem)(_System = OSSystem.Windows);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        return (OSSystem)(_System = OSSystem.Linux);
                    return (OSSystem)(_System = OSSystem.MacOS);
                }
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
