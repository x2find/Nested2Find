using EPiServer.Find.Api.Querying;
using EPiServer.Find.Helpers;
using Newtonsoft.Json;

namespace Nested2Find.Api.Querying.Filters
{
    [JsonConverter(typeof(NestedFilterConverter))]
    public class NestedFilter : Filter
    {
        public NestedFilter(string path, Filter filter)
        {
            path.ValidateNotNullArgument("path");
            filter.ValidateNotNullArgument("filter");

            Path = path;
            Filter = filter;
        }

        public string Path { get; set; }

        public Filter Filter { get; set; }

        public bool? Join { get; set; }
    }
}
