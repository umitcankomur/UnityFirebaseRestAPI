using Newtonsoft.Json;
using UnityEngine;

namespace FirebaseRestAPI
{
    public class CollectionSelector
    {
        [JsonProperty("collectionId")]
        public string CollectionId { get; set; }
    }
}

