using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace FirebaseRestAPI
{
    public class Order
    {
        [JsonProperty("field")]
        public FieldReference Field { get; set; }

        [JsonProperty("direction")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderDirection Direction { get; set; }
    }
}
