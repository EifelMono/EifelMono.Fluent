using System.Collections.Generic;
using System.IO;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public static class ValuePathExtensions
    {
        internal static string NormalizePath(this string thisValue)
            => (thisValue ?? "")
            .Replace("\\", Path.DirectorySeparatorChar.ToString())
            .Replace("/", Path.DirectorySeparatorChar.ToString());

        public static T MakeNormalizePath<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.Value.NormalizePath();
            return thisValue;
        }

        public static T MakeFullPath<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.FullPath;
            return thisValue;
        }
        public static T MakeAbsolute<T>(this T thisValue) where T : ValuePath
            => thisValue.MakeFullPath();

        public static T IfEndsWithPathThenRemove<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.Value.IfEndsWithRemove(ValuePath.PathSeparatorChar.ToString());
            return thisValue;
        }

        public static bool EndsWithPath<T>(this T thisValue) where T : ValuePath
            => thisValue.Value.EndsWith(ValuePath.PathSeparatorChar.ToString());

  
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
