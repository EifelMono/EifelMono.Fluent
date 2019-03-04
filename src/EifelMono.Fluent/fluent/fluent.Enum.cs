using System.Collections.Generic;
using System.Linq;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class Enum
        {
            public static IEnumerable<T> Values<T>() where T : System.Enum
               => System.Enum.GetValues(typeof(T)).Cast<T>();
            public static IEnumerable<string> Names<T>() where T : System.Enum
               => System.Enum.GetNames(typeof(T)).Cast<string>();

            public static string Name<T>(object value) where T : System.Enum
                => System.Enum.GetName(typeof(T), value);
            public static bool IsDefined<T>(object value) where T : System.Enum
                => System.Enum.IsDefined(typeof(T), value);

            public static T Parse<T>(string value, bool ignoreCase = false) where T : System.Enum
                => (T)System.Enum.Parse(typeof(T), value, ignoreCase);

            public static (bool Ok, T Value) SafeParse<T>(string value, bool ignoreCase = false, T defaultValue = default) where T : System.Enum
            {
                // T data = default(T);
                // var ok = System.Enum.TryParse<T>(value, ignoreCase, out data);
                // return (ok, data);
                try
                {
                    return (true, Parse<T>(value, ignoreCase));
                }
                catch
                {
                    return (false, defaultValue);
                }
            }

            public static T TryParse<T>(string value, bool ignoreCase = false, T defaultValue = default) where T : System.Enum
                => SafeParse<T>(value, ignoreCase, defaultValue).Value;
        }
    }
}
