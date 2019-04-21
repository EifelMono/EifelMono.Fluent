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
                return onError != null ? onError.Invoke(thisValue, null) : default;
            try
            {
                return thisValue.ReadAllText().FromJson<T>();
            }
            catch (Exception ex)
            {
                return onError != null ? onError.Invoke(thisValue, ex) : default;
            }
        }
    }
}
