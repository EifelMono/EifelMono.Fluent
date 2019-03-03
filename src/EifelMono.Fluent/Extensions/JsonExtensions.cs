﻿using System;
using Newtonsoft.Json;

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

        public static T FormJson<T>(this string thisValue)
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
            => (T)JsonConvert.DeserializeObject<JsonEnvelope>(thisValue).Data;
    }
}
