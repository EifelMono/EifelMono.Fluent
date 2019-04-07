using System;
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

            public static (bool Ok, T Value) Value<T>(int value) where T : System.Enum
            {
                var values = Values<T>().Where(v => { var x = (int)Convert.ChangeType(v, typeof(int)); return x == value; });
                if (values.Count() == 1)
                    return (true, values.First());
                return (false, default);
            }

            public static (bool Ok, string Value) Name<T>(int value) where T : System.Enum
            {
                if (TrySafe(() => System.Enum.GetName(typeof(T), value)) is var result && result.Ok && result.Value != null)
                    return result;
                else
                    return (false, default);
            }
            public static bool IsDefined<T>(object value) where T : System.Enum
                => System.Enum.IsDefined(typeof(T), value);

            public static T Parse<T>(string value, bool ignoreCase = false) where T : System.Enum
                => (T)System.Enum.Parse(typeof(T), value, ignoreCase);

            public static (bool Ok, T Value) ParseSafe<T>(string value, bool ignoreCase = false, T defaultValue = default) where T : System.Enum
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
                => ParseSafe<T>(value, ignoreCase, defaultValue).Value;
        }
    }
}
