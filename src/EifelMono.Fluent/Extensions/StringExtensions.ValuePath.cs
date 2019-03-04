using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using EifelMono.Fluent.IO;

[assembly: InternalsVisibleTo("EifelMono.Fluent.Test")]
namespace EifelMono.Fluent.Extensions
{
    public static partial class StringExtensions
    {
        internal static string NormalizePath(this string thisValue)
          => (thisValue ?? "")
            .Replace("\\", Path.DirectorySeparatorChar.ToString())
            .Replace("/", Path.DirectorySeparatorChar.ToString());

        internal static IEnumerable<string> SplitPath(this string thisValue)
            => (thisValue ?? "").Split(Path.DirectorySeparatorChar);
        internal static string JoinPath(this List<string> thisValue)
            => string.Join(Path.DirectorySeparatorChar.ToString(), thisValue);

        public static FilePath AsFilePath(this string thisValue)
            => new FilePath(thisValue);
        public static string AsDirectoryPath(this string thisValue)
            => new DirectoryPath(thisValue);
    }
}
