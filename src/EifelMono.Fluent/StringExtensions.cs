using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EifelMono.Fluent.Test")]
namespace EifelMono.Fluent
{
    internal static class InternalStringExtensions
    {
        public static string NormalizePath(this string thisValue)
            => thisValue
              .Replace("\\", Path.DirectorySeparatorChar.ToString())
              .Replace("/", Path.DirectorySeparatorChar.ToString());
    }
    public static class StringExtensions
    {
    }
}
