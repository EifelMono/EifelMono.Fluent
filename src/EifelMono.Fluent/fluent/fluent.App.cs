using System;
using System.Reflection;
using System.Runtime.Versioning;
using EifelMono.Fluent.IO;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class App
        {
            public static FilePath Executable
                => FilePath.OS.Current;

            // <Version> 1.0.1 </Version>
            public static string Version
                => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            // <AssemblyVersion>1.0.2.0</AssemblyVersion>
            public static string AssemblyVersion
                => Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "";
            // => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "";

            // <FileVersion>1.0.3.0</FileVersion>
            public static string FileVersion
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "";

            // <Company>Company</Company>
            public static string Company
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";

            // <Copyright>Copyright</Copyright>
            public static string Copyright
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? "";

            // <Title>Copyright</Title>
            public static string Title
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";

            // <Product>Copyright</Product>
            public static string Product
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "";

            public static string Configuration
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? "";

            public static string Culture
             => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCultureAttribute>()?.Culture?? "";

            public static string Description
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";

            public static string Trademark
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark?? "";

            public static string FrameworkName
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? "";
            public static string FrameworkDisplayName
                => Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName ?? "";

        }
    }
}
