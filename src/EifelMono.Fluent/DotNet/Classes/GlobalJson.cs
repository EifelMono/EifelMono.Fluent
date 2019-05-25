using EifelMono.Fluent.Extensions;
using Newtonsoft.Json;

namespace EifelMono.Fluent.DotNet.Classes
{
    public class GlobalJson
    {
        public class SdkClass
        {
            [JsonProperty("version")]
            public string Version { get; set; }
        }

        public GlobalJson(string version = null)
        {
            if (version.IsNotNullOrEmpty())
                Sdk = new SdkClass { Version = version };
        }
   
        [JsonProperty("sdk")]
        public SdkClass Sdk { get; set; } = new SdkClass();
    }
}
