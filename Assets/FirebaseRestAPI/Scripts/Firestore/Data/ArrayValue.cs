using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class ArrayValue
    {
        [JsonProperty("values")]
        public Value[] Values { get; set; }
    }
}

