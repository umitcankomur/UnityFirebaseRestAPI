using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class StructuredQuery
    {
        [JsonProperty("from")]
        public CollectionSelector From { get; set; }

        [JsonProperty("orderBy", NullValueHandling = NullValueHandling.Ignore)]
        public Order[] OrderBy { get; set; }

        [JsonProperty("startAt", NullValueHandling = NullValueHandling.Ignore)]
        public Cursor StartAt { get; set; }

        [JsonProperty("endAt", NullValueHandling = NullValueHandling.Ignore)]
        public Cursor EndAt { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        [JsonProperty("where", NullValueHandling = NullValueHandling.Ignore)]
        public Filter Where { get; set; }
    }
}