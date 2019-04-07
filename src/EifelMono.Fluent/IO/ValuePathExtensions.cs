using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public static class ValuePathExtensions
    {
        public static T MakeNormalizePath<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.Value.NormalizePath();
            return thisValue;
        }

        public static T MakeAbsolute<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.FullPath;
            return thisValue;
        }
        public static T MakeFullPath<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.FullPath;
            return thisValue;
        }

        public static T IfEndsWithPathThenRemove<T>(this T thisValue) where T : ValuePath
        {
            thisValue.Value = thisValue.Value.IfEndsWithRemove(ValuePath.PathSeparatorChar.ToString());
            return thisValue;
        }

        public static bool EndsWithPath<T>(this T thisValue) where T : ValuePath
            => thisValue.Value.EndsWith(ValuePath.PathSeparatorChar.ToString());
    }
}
