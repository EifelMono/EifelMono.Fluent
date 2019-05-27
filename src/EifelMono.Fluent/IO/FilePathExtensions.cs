using System;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Log;

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

        public static (bool Ok, T Value, FilePath FluentValue) ReadJsonSafe<T>(this FilePath thisValue, bool useDefaultOnNotExist = true)
        {
            try
            {
                if (!thisValue.Exists)
                {
                    if (useDefaultOnNotExist)
                        return (true, fluent.Default<T>(), thisValue);
                    return (false, default, thisValue);
                }
                return (true, thisValue.ReadAllText().FromJson<T>(), thisValue);
            }
            catch (Exception ex)
            {
                ex.LogSafeException();
            }
            return (false, default, thisValue);
        }
    }
}
