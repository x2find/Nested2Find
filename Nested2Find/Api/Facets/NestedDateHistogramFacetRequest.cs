using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find.Api.Facets;
using Newtonsoft.Json;

namespace Nested2Find.Api.Facets
{
    [JsonConverter(typeof(NestedDateHistogramFacetRequestConverter))]
    public class NestedDateHistogramFacetRequest : NestedFacetRequest
    {
        public NestedDateHistogramFacetRequest(string name)
            : base(name)
        {
            PreTimeZone = TimeZoneInfo.Local;
        }

        public DateInterval Interval { get; set; }

        public TimeZoneInfo PreTimeZone { get; set; }
    }
}
