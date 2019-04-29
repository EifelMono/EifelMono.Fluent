using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EifelMono.Fluent.Nuget.Classes
{
    public class AutoCompleteContext
    {
        [JsonProperty("@vocab")]
        public string Vocab { get; set; }
    }

    public class AutoComplete
    {
        [JsonProperty("@context")]
        public AutoCompleteContext Context { get; set; }
        [JsonProperty("totalHits")]
        public int TotalHits { get; set; }
        [JsonProperty("lastReopen")]
        public DateTime LastReopen { get; set; }
        [JsonProperty("index")]
        public string Index { get; set; }
        [JsonProperty("data")]
        public List<string> Data { get; set; }
    }
}
