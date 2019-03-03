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

            public static T Parse<T>(string value) where T : System.Enum
                => (T)System.Enum.Parse(typeof(T), value);
            public static T Parse<T>(string value, bool ignoreCase) where T : System.Enum
                => (T)System.Enum.Parse(typeof(T), value, ignoreCase);

            public static (bool Ok, T Data) SafeTryParse<T>(string value) where T : System.Enum
            {
                // T data = default(T);
                // var ok = System.Enum.TryParse<T>(value, out data);
                // return (ok, data);
                try
                {
                    return (true, Parse<T>(value));
                }
                catch
                {
                    return (false, default);
                }
            }
            public static T TryParse<T>(string value) where T : System.Enum
                => SafeTryParse<T>(value).Data;

            public static (bool Ok, T Data) SafeTryParse<T>(string value, bool ignoreCase) where T : System.Enum
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
                    return (false, default);
                }
            }
        }
    }
}
