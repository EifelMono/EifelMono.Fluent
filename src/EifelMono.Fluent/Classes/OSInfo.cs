using System.Runtime.InteropServices;

namespace EifelMono.Fluent.Classes
{
    public class OSInfo
    {
        public OSPlatform CurrentPlatform
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
        public bool IsWindows
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public bool IsOSX
            => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public bool IsLinux
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public string FrameworkDescription
            => RuntimeInformation.FrameworkDescription;
        public Architecture OSArchitecture
            => RuntimeInformation.OSArchitecture;
        public string OSDescription
            => RuntimeInformation.OSDescription;
        public Architecture ProcessArchitecture
            => RuntimeInformation.ProcessArchitecture;
    }
}
