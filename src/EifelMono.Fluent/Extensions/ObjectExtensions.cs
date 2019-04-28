using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object thisValue)
            => thisValue is null;
        public static bool IsNotNull(this object thisValue)
            => thisValue is object;
    }
}
