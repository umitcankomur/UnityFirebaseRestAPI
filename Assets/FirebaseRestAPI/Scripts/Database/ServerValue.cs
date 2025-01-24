using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class ServerValue
    {
        [JsonProperty(".sv")]
        public object Value { get; set; }

        public static ServerValue TIMESTAMP
        {
            get
            {
                ServerValue serverValue = new ServerValue();
                serverValue.Value = "timestamp";
                return serverValue;
            }
        }

        public static ServerValue Increment(int increment)
        {
            ServerValue serverValue = new ServerValue();
            serverValue.Value = new IncrementServerValue { Increment = increment };
            return serverValue;
        }

        public class IncrementServerValue
        {
            [JsonProperty("increment")]
            public int Increment { get; set; }
        }
    }
}
