﻿using System.Reflection;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Extensions
{
    public static class AssemblyExtensions
    {
        public static AssemblyInfo AssemblyInfo(this Assembly thisValue)
            => new AssemblyInfo(thisValue);
    }
}
