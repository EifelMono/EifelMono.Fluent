using System;
using Newtonsoft.Json;

namespace EifelMono.Fluent.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object thisValue, Formatting formatting = Formatting.Indented, FluentExAction<object, string> fluentExAction = default)
        {
            try
            {
                return JsonConvert.SerializeObject(thisValue, Formatting.Indented);
            }
            catch (Exception ex)
            {
                if (fluentExAction?.Invoke(ex, thisValue) is var result && result != null && result.Fixed)
                    return result.Data;
                throw ex;
            }
        }

        public static T FormJson<T>(this string thisValue, FluentExAction<string, T> fluentExAction = default)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(thisValue);
            }
            catch (Exception ex)
            {
                if (fluentExAction?.Invoke(ex, thisValue) is var result && result != null && result.Fixed)
                    return result.Data;
                throw ex;
            }
        }


        public class JsonEnvelope
        {
            public string Name { get; set; }
            public object Data { get; set; }

            public static JsonEnvelope Create(object data)
                => new JsonEnvelope { Name = data?.GetType().Name ?? "", Data = data };
        }
        public static string ToJsonEnvelope(this object thisValue, Formatting formatting = Formatting.Indented, FluentExAction<object, string> fluentExAction = default)
        {
            try
            {
                return JsonConvert.SerializeObject(JsonEnvelope.Create(thisValue), Formatting.Indented);
            }
            catch (Exception ex)
            {
                if (fluentExAction?.Invoke(ex, thisValue) is var result && result != null && result.Fixed)
                    return result.Data;
                throw ex;
            }
        }

        public static T FormJsonEnvelope<T>(this string thisValue, FluentExAction<string, T> fluentExAction = default)
        {
            try
            {
                return (T)JsonConvert.DeserializeObject<JsonEnvelope>(thisValue).Data;
            }
            catch (Exception ex)
            {
                if (fluentExAction?.Invoke(ex, thisValue) is var result && result != null && result.Fixed)
                    return result.Data;
                throw ex;
            }
        }

    }
}
