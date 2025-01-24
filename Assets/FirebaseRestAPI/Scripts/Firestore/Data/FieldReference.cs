using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class FieldReference
    {
        [JsonProperty("fieldPath")]
        public string FieldPath { get; set; }
    }
}
