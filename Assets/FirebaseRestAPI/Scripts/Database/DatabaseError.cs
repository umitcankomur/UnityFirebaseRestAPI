
using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class DatabaseError
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
