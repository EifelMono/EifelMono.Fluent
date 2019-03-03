using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Extensions
{
    public static class AssemblyExtensions
    {
        public static AssemblyInfo AssemblyInfo(this Assembly thisValue)
            => new AssemblyInfo(thisValue);

        public static List<AssemblyInfo.AttributeItem> AssemblyInfos(this Assembly thisValue)
            => new AssemblyInfo(thisValue).AllInfos();
    }
}
