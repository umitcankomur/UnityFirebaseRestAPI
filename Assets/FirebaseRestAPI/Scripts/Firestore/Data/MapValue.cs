using Newtonsoft.Json;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public class MapValue
    {
        [JsonProperty("fields")]
        public Dictionary<string, Value> Fields { get; set; }
    }
}
