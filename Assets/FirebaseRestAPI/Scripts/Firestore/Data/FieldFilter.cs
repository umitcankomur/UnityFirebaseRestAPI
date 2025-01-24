using FirebaseRestAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace FirebaseRestAPI
{
    public class FieldFilter
    {
        [JsonProperty("field")]
        public FieldReference Field { get; set; }

        [JsonProperty("op")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldOperator Operator { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }
}
