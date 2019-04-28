﻿using System;
using System.Collections.Generic;
using System.Reflection;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.IO;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static FilePath Executable
            => FilePath.OS.Current;

        private static OSInfo s_osInfo = null;
        public static OSInfo OS => s_osInfo ?? (s_osInfo = new OSInfo());

        private static AssemblyInfo s_app = null;
        public static AssemblyInfo App => s_app ?? (s_app = new AssemblyInfo(Assembly.GetEntryAssembly()));

        private static AssemblyInfo s_fluentLib = null;
        public static AssemblyInfo FluentLib
            => s_fluentLib ?? (s_fluentLib = AssemblyInfo.FromType(typeof(fluent)));

#pragma warning disable IDE1006 // Naming Styles
        public static T[] @params<T>(params T[] values)
#pragma warning restore IDE1006 // Naming Styles
            => values;
    }
}
