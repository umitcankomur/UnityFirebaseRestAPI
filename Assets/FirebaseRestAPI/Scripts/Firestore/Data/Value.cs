using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FirebaseRestAPI
{
    public class Value
    {
        [JsonProperty("stringValue", NullValueHandling = NullValueHandling.Ignore)]
        public string StringValue { get; set; }

        [JsonProperty("integerValue", NullValueHandling = NullValueHandling.Ignore)]
        public string IntegerValue { get; set; }

        [JsonProperty("doubleValue", NullValueHandling = NullValueHandling.Ignore)]
        public string DoubleValue { get; set; }

        [JsonProperty("timestampValue", NullValueHandling = NullValueHandling.Ignore)]
        public string TimestampValue { get; set; }

        [JsonProperty("booleanValue", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BooleanValue { get; set; }

        [JsonProperty("nullValue", NullValueHandling = NullValueHandling.Ignore)]
        public string NullValue { get; set; }

        [JsonProperty("arrayValue", NullValueHandling = NullValueHandling.Ignore)]
        public ArrayValue ArrayValue { get; set; }

        [JsonProperty("mapValue", NullValueHandling = NullValueHandling.Ignore)]
        public MapValue MapValue { get; set; }

        public static Value ObjectToValue(object obj)
        {
            var type = obj.GetType();

            if (type == typeof(int))
            {
                return new Value { IntegerValue = obj.ToString() };
            }
            else if (type == typeof(bool))
            {
                return new Value { BooleanValue = (bool)obj };
            }
            else if (type == typeof(double) || type == typeof(float) || type == typeof(decimal))
            {
                return new Value { DoubleValue = obj.ToString() };
            }
            else if (type == typeof(string))
            {
                return new Value { StringValue = (string)obj };
            }
            else if (type == typeof(DateTime))
            {
                return new Value { TimestampValue = ((DateTime)obj).ToString("o") };
            }
            else if (type == typeof(List<object>))
            {
                return new Value
                {
                    ArrayValue = new ArrayValue
                    {
                        Values = ((List<object>)obj).Select(x => ObjectToValue(x)).ToArray()
                    }
                };
            }
            else if (type == typeof(object[]))
            {
                return new Value
                {
                    ArrayValue = new ArrayValue
                    {
                        Values = ((object[])obj).Select(x => ObjectToValue(x)).ToArray()
                    }
                };
            }
            else if (type == typeof(Dictionary<string, object>))
            {
                return new Value
                {
                    MapValue = new MapValue
                    {
                        Fields = ((Dictionary<string, object>)obj).ToDictionary(x => x.Key, x => ObjectToValue(x.Value))
                    }
                };
            }
            else
            {
                throw new Exception("Unsupported type");
            }
        }

        public object ToObject()
        {
            if (StringValue != null)
            {
                return StringValue;
            }
            else if (IntegerValue != null)
            {
                return int.Parse(IntegerValue);
            }
            else if (DoubleValue != null)
            {
                return double.Parse(DoubleValue);
            }
            else if (TimestampValue != null)
            {
                return DateTime.Parse(TimestampValue);
            }
            else if (BooleanValue != null)
            {
                return BooleanValue;
            }
            else if (NullValue != null)
            {
                return null;
            }
            else if (ArrayValue != null)
            {
                return ArrayValue.Values.Select(x => x.ToObject()).ToArray();
            }
            else if (MapValue != null)
            {
                return MapValue.Fields.ToDictionary(x => x.Key, x => x.Value.ToObject());
            }
            else
            {
                throw new Exception("Unsupported type");
            }
        }
    }

}