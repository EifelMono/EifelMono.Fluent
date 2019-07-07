using System;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Log;
#if NETSTANDARD2_1_PLUS
using EifelMono.Fluent.Classes;
using System.Runtime.CompilerServices;
#endif

namespace EifelMono.Fluent.IO
{
    public static class FilePathExtensions
    {
        public static FilePath WriteJson(this FilePath thisValue, object value, Action<FilePath, Exception, object> onError = null)
        {
            try
            {
                thisValue.WriteAllText(value.ToJson());
            }
            catch (Exception ex)
            {
                if (onError is object)
                    onError(thisValue, ex, value);
            }
            return thisValue;
        }

        public static (bool Ok, FilePath FluentValue) WriteJsonSafe(this FilePath thisValue, object value)
        {
            try
            {
                thisValue.WriteAllText(value.ToJson());
                return (true, thisValue);
            }
            catch (Exception ex)
            {
                ex.LogSafeException();
            }
            return (false, thisValue);
        }

#if NETSTANDARD2_1_PLUS
        public static (bool Ok, FilePath FluentValue) WriteJsonSafe<T>(this T thisValue, object value) where T : ITuple
        {
            if (new SafeTuple<FilePath>(thisValue) is var safeTuple && !safeTuple.Ok)
                return (safeTuple.Ok, safeTuple.ThisValue);
            try
            {
                safeTuple.ThisValue.WriteAllText(value.ToJson());
                return (true, safeTuple.ThisValue);
            }
            catch (Exception ex)
            {
                ex.LogSafeException();
            }
            return (false, safeTuple.ThisValue);
        }
#endif
        public static T ReadJson<T>(this FilePath thisValue, Func<FilePath, Exception, T> onError = null)
        {
            if (!thisValue.Exists)
                return onError is object ? onError.Invoke(thisValue, null) : fluent.Default<T>();
            try
            {
                return thisValue.ReadAllText().FromJson<T>();
            }
            catch (Exception ex)
            {
                return onError is object ? onError.Invoke(thisValue, ex) : fluent.Default<T>();
            }
        }

        public static (bool Ok, T Value, bool Exist, FilePath ThisValue) ReadJsonSafe<T>(this FilePath thisValue, bool useDefaultOnNotExist = true)
        {
            bool exist = false;
            try
            {
                exist = thisValue.Exists;
                if (!exist)
                {
                    if (useDefaultOnNotExist)
                        return (true, fluent.Default<T>(), exist, thisValue);
                    return (false, default, exist, thisValue);
                }
                return (true, thisValue.ReadAllText().FromJson<T>(), exist, thisValue);
            }
            catch (Exception ex)
            {
                ex.LogSafeException();
            }
            return (false, default, exist, thisValue);
        }
#if NETSTANDARD2_1_PLUS
        public static (bool Ok, T Value, bool Exist, FilePath ThisValue) ReadJsonSafe<T>(this ITuple thisValue, bool useDefaultOnNotExist = true)
        {
            if (new SafeTuple<FilePath>(thisValue) is var safeTuple && !safeTuple.Ok)
                return (safeTuple.Ok, default, false, safeTuple.ThisValue);
            bool exist = false;
            try
            {
                exist = safeTuple.ThisValue.Exists;
                if (!exist)
                {
                    if (useDefaultOnNotExist)
                        return (true, fluent.Default<T>(), exist, safeTuple.ThisValue);
                    return (false, default, exist, safeTuple.ThisValue);
                }
                return (true, safeTuple.ThisValue.ReadAllText().FromJson<T>(), exist, safeTuple.ThisValue);
            }
            catch (Exception ex)
            {
                ex.LogSafeException();
            }
            return (false, default, exist, safeTuple.ThisValue);
        }
#endif
    }
}
