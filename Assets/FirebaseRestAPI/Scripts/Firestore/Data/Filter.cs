using Newtonsoft.Json;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public class Filter
    {
        [JsonProperty("compositeFilter", NullValueHandling = NullValueHandling.Ignore)]
        public CompositeFilter CompositeFilter { get; set; }

        [JsonProperty("fieldFilter", NullValueHandling = NullValueHandling.Ignore)]
        public FieldFilter FieldFilter { get; set; }

        public static Filter And(List<Filter> filters)
        {
            Filter filter = new Filter();
            filter.CompositeFilter = new CompositeFilter()
            {
                Filters = filters,
                FilterOperator = CompositeFilterOperator.AND
            };
            return filter;
        }

        public static Filter Or(List<Filter> filters)
        {
            Filter filter = new Filter();
            filter.CompositeFilter = new CompositeFilter()
            {
                Filters = filters,
                FilterOperator = CompositeFilterOperator.OR
            };
            return filter;
        }

        public static Filter Field(string fieldPath, FieldOperator fieldOperator, object value)
        {
            Filter fieldFilter = new Filter();
            fieldFilter.FieldFilter = new FieldFilter()
            {
                Field = new FieldReference()
                {
                    FieldPath = fieldPath
                },
                Operator = fieldOperator,
                Value = Value.ObjectToValue(value)
            };
            return fieldFilter;
        }
    }
}

