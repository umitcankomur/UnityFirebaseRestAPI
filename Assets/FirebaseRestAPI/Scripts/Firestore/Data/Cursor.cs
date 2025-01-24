using Newtonsoft.Json;


namespace FirebaseRestAPI
{
    public class Cursor
    {
        [JsonProperty("values")]
        public Value[] Values { get; set; }
    }
}
