using System;
using System.Reflection;
using System.Runtime.Versioning;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        private static AssemblyInfo s_app = null;
        public static AssemblyInfo App
        {
            get => s_app ?? (s_app = new AssemblyInfo(Assembly.GetEntryAssembly()));
        }

        public static FilePath Executable
            => FilePath.OS.Current;

        private static AssemblyInfo s_fluentLib = null;
        public static AssemblyInfo FluentLib
        {
#if NETSTANDARD2_0
            get => s_fluentLib ?? (s_fluentLib = new AssemblyInfo(typeof(fluent).Assembly));
#endif
#if NETSTANDARD1_6
            get => s_fluentLib ?? (s_fluentLib = new AssemblyInfo(typeof(fluent).GetTypeInfo().Assembly));
#endif
        }






    }
}
