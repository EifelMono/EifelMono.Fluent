using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.IO
{
    public static class ValuePathExtensions
    {
        public static T NormalizePath<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.Value.NormalizePath();
            return thisValue;
        }

        public static T MakeAbsolute<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.FullPath;
            return thisValue;
        }
    }
}
