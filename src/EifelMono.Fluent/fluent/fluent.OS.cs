using System.Runtime.InteropServices;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class OS
        {
            public static OSPlatform CurrentPlatform
            {
                get
                {
                    if (IsWindows)
                        return OSPlatform.Windows;
                    if (IsOSX)
                        return OSPlatform.OSX;
                    return OSPlatform.Linux;
                }
            }
            public static bool IsWindows
                => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            public static bool IsOSX
                => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            public static bool IsLinux
                => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            public static string FrameworkDescription
                => RuntimeInformation.FrameworkDescription;
            public static Architecture OSArchitecture
                => RuntimeInformation.OSArchitecture;
            public static string OSDescription
                => RuntimeInformation.OSDescription;
            public static Architecture ProcessArchitecture
                => RuntimeInformation.ProcessArchitecture;
        }
    }
}
