using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class Query
    {
        [JsonProperty("structuredQuery")]
        public StructuredQuery StructuredQuery { get; set; }
    }
}
