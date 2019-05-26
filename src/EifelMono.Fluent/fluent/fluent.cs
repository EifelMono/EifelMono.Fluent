using System;
using System.Collections.Generic;
using System.Reflection;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{
    public static partial class fluent
    {
        public static FilePath Executable
            => FilePath.OS.Current;

        private static OSInfo s_osInfo = null;
        public static OSInfo OSInfo => s_osInfo ?? (s_osInfo = new OSInfo());

        private static AssemblyInfo s_app = null;
        public static AssemblyInfo App => s_app ?? (s_app = new AssemblyInfo(Assembly.GetEntryAssembly()));

        private static AssemblyInfo s_fluentLib = null;
        public static AssemblyInfo FluentLib
            => s_fluentLib ?? (s_fluentLib = AssemblyInfo.FromType(typeof(fluent)));

#pragma warning disable IDE1006 // Naming Styles
        public static T[] @params<T>(params T[] values)
#pragma warning restore IDE1006 // Naming Styles
            => values;

        public static T Default<T>()
            => (T)Default(typeof(T));

        public static object Default(Type type)
        {
#if NETSTANDARD1_6
            var infoType = type.GetTypeInfo();
#else
            var infoType = type;
#endif
            try
            {
                if (infoType.IsValueType)
                    return Activator.CreateInstance(type);
                if (infoType.IsClass)
                {
                    if (type == typeof(string))
                        return "";
                    return Activator.CreateInstance(type);
                }
            }
            catch { }
            return default;
        }
    }
}
