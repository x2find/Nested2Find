using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;

namespace Nested2Find.Api.Facets
{
    public abstract class NestedFacetRequest : FacetRequest
    {
        public NestedFacetRequest(string name)
            : base(name)
        {
        }

        public string Field { get; set; }

        public string Nested { get; set; }

        public Filter FacetFilter { get; set; }
    }
}
