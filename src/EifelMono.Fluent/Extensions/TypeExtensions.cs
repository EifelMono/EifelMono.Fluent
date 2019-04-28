using System;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Extensions
{
    public static class TypeExtensions
    {
        public static AssemblyInfo AssemblyInfo(this Type thisValue)
            => Classes.AssemblyInfo.FromType(thisValue);
    }
}
