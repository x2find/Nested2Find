using EPiServer.Find.Api;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Json;
using Newtonsoft.Json;
namespace Nested2Find.Api
{
    public enum SortMode
    {
        Min, 
        Max,
        Avg,
        Sum    
    }

    [JsonConverter(typeof(NestedSortingConverter))]
    public class NestedSorting : ISorting
    {
        public NestedSorting(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; private set; }

        public SortOrder? Order { get; set; }

        public SortMissing? Missing { get; set; }

        public SortMode? Mode { get; set; }

        public bool? IgnoreUnmapped { get; set; }

        public string NestedPath { get; set; }

        public Filter NestedFilter { get; set; }
    }
}
