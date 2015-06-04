using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using Newtonsoft.Json;

namespace Nested2Find.Api.Facets
{
    [JsonConverter(typeof(NestedHistogramFacetRequestConverter))]
    public class NestedHistogramFacetRequest : NestedFacetRequest
    {
        public NestedHistogramFacetRequest(string name)
            : base(name)
        {
        }

        public string KeyField { get; set; }

        public string ValueField { get; set; }

        public string KeyScript { get; set; }

        public string ValueScript { get; set; }

        public int? Interval { get; set; }
    }
}
