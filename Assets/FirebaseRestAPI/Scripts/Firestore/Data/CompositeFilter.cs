using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public class CompositeFilter
    {
        [JsonProperty("op")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CompositeFilterOperator FilterOperator { get; set; }

        [JsonProperty("filters")]
        public List<Filter> Filters { get; set; }
    }
}

