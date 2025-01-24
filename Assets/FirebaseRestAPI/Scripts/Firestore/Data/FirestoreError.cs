using Newtonsoft.Json;
using UnityEngine;

namespace FirebaseRestAPI
{
    public class FirestoreError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

