using EPiServer.Find.Api.Facets;
using Newtonsoft.Json;

namespace Nested2Find.Api.Facets
{
    [JsonConverter(typeof(NestedHistogramFacetRequestConverter))]
    public class NestedHistogramFacetRequest : FacetRequest
        {
        public NestedHistogramFacetRequest(string name)
            : base(name)
        {
        }

        public string Field { get; set; }

        public string KeyField { get; set; }

        public string ValueField { get; set; }

        public string KeyScript { get; set; }

        public string ValueScript { get; set; }

        public int? Interval { get; set; }

        public string Nested { get; set; }
    }
}
