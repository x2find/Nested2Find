using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find.Api.Facets;
using Newtonsoft.Json;

namespace Nested2Find.Api.Facets
{
    [JsonConverter(typeof(NestedTermsFacetRequestConverter))]
    public class NestedTermsFacetRequest : FacetRequest
    {
        public NestedTermsFacetRequest(string name) 
            : base(name)
        {
        }

        public string Field { get; set; }

        public IEnumerable<string> Fields { get; set; }

        public int? Size { get; set; }

        public string Script { get; set; }

        public bool AllTerms { get; set; }

        public string Nested { get; set; }
    }
}
