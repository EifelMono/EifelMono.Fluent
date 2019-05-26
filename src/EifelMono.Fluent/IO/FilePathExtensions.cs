using System;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public static class FilePathExtensions
    {
        public static FilePath WriteJson(this FilePath thisValue, object value)
            => thisValue.WriteAllText(value.ToJson());

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
    }
}
