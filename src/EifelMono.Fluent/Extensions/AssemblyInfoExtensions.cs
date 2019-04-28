using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Extensions
{
    public static class AssemblyInfoExtensions
    {
        public static string CustomAttributesAsJson(this AssemblyInfo thisValue)
            => thisValue.CustomAttributesAsJson();
    }
}
