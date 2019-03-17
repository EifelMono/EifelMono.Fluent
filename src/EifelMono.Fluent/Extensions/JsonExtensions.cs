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

        public static object FromJson(this string thisValue, Type type)
            => JsonConvert.DeserializeObject(thisValue, type);

        public static T FromJson<T>(this string thisValue)
            => JsonConvert.DeserializeObject<T>(thisValue);

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
            var envelopMessage = JsonConvert.DeserializeObject<JsonEnvelope>(thisValue);
            return (envelopMessage.Data as JObject).ToObject(type);
        }

        public static (string Name, T Data) FormJsonEnvelopeAsEnvelop<T>(this string thisValue)
        {
            var envelopMessage = JsonConvert.DeserializeObject<JsonEnvelope>(thisValue);
            return (envelopMessage.Name, (T)(envelopMessage.Data as JObject).ToObject(typeof(T)));
        }

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
