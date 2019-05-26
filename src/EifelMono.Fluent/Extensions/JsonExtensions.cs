using System;
using EifelMono.Fluent.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EifelMono.Fluent.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object thisValue, bool indented = true, bool defaults = true)
        {
            return JsonConvert.SerializeObject(thisValue, new JsonSerializerSettings
            {
                Formatting = indented ? Formatting.Indented : Formatting.None,
                DefaultValueHandling = defaults ? DefaultValueHandling.Include : DefaultValueHandling.Ignore
            });
        }

        public static object FromJson(this string thisValue, Type type, Func<string, Exception, object> onError = null)
        {
            try
            {
                return JsonConvert.DeserializeObject(thisValue, type);
            }
            catch (Exception ex)
            {
                return onError is object ? onError.Invoke(thisValue, ex) : fluent.Default(type);
            }
        }

        public static T FromJson<T>(this string thisValue, Func<string, Exception, T> onError = null)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(thisValue);
            }
            catch (Exception ex)
            {
                return onError is object ? onError.Invoke(thisValue, ex) : fluent.Default<T>();
            }
        }

        public class JsonEnvelope
        {
            public string Name { get; set; }
            public object Data { get; set; }

            public static JsonEnvelope Create(object data)
                => new JsonEnvelope { Name = data?.GetType().Name ?? "", Data = data };
        }
        public static string ToJsonEnvelope(this object thisValue, bool indented = true, bool defaults = true)
            => JsonEnvelope.Create(thisValue).ToJson(indented, defaults);
        public static T FormJsonEnvelope<T>(this string thisValue)
        {
            var envelopMessage = JsonConvert.DeserializeObject<JsonEnvelope>(thisValue);
            return (T)(envelopMessage.Data as JObject).ToObject(typeof(T));
        }

        public static object FormJsonEnvelope(this string thisValue, Type type)
        {
            var envelopMessage = thisValue.FromJson<JsonEnvelope>();
            return (envelopMessage.Data as JObject).ToObject(type);
        }

        public static (string Name, T Data) FormJsonEnvelopeAsEnvelop<T>(this string thisValue)
        {
            var envelopMessage = thisValue.FromJson<JsonEnvelope>();
            return (envelopMessage.Name, (T)(envelopMessage.Data as JObject).ToObject(typeof(T)));
        }
    }
}
