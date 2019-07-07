using System;
using System.Reflection;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Extensions
{
    public static class TypeExtensions
    {
        public static AssemblyInfo AssemblyInfo(this Type thisValue)
            => Classes.AssemblyInfo.FromType(thisValue);

#if NETSTANDARD1_6
        public static bool IsSubclassOf(this Type thisValue, Type inheritedType)
            => inheritedType.IsAssignableFrom(thisValue);
#endif
    }
}
