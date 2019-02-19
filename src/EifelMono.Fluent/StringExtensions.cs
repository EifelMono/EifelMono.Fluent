using System.Collections.Generic;
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

        public static IEnumerable<string> SplittPath(this string thisValue)
            => (thisValue ?? "").Split(Path.DirectorySeparatorChar);
    }
}
