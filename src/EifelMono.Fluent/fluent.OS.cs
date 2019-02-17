using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class OS
        {
            public static bool IsWindows
                => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            public static bool IsOSX
                => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            public static bool IsLinux
                => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}
