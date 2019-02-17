using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class Path
        {
            public static string Normalize(string value)
                => value
                .Replace("\\", System.IO.Path.DirectorySeparatorChar.ToString())
                .Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
        }
    }
}
