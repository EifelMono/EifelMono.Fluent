﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;


namespace EifelMono.Fluent
{
    public static class StringExtensions
    {
        public static string NormalizePath(this string thisValue)
           => (thisValue ?? "")
           .Replace("\\", Path.DirectorySeparatorChar.ToString())
           .Replace("/", Path.DirectorySeparatorChar.ToString());

        public static IEnumerable<string> SplitPath(this string thisValue)
            => (thisValue ?? "").Split(Path.DirectorySeparatorChar);
        public static string ToPath(this List<string> thisValue)
            => string.Join(Path.DirectorySeparatorChar.ToString(), thisValue);
    }
}