using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public class Document
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fields")]
        public Dictionary<string, Value> Fields { get; set; }

        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }
    }
}

