using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Text;
using Newtonsoft.Json;

namespace Nested2Find.Api.Facets
{
    public class NestedDateHistogramFacetRequestConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var facetRequest = (NestedDateHistogramFacetRequest)value;
            writer.WriteStartObject();
            writer.WritePropertyName("date_histogram");
            writer.WriteStartObject();
            if (facetRequest.Field.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("field");
                writer.WriteValue(facetRequest.Field);
            }
            if (facetRequest.Interval.IsNotNull())
            {
                writer.WritePropertyName("interval");
                writer.WriteValue(facetRequest.Interval.ToString().ToLowerInvariant());
            }
            if (facetRequest.PreTimeZone.IsNotNull())
            {
                writer.WritePropertyName("pre_zone");
                writer.WriteValue(facetRequest.PreTimeZone.BaseUtcOffset.ToString(@"hh\:mm"));
            }
            writer.WriteEndObject();
            if (facetRequest.Nested.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("nested");
                writer.WriteValue(facetRequest.Nested);
            }
            if (facetRequest.FacetFilter.IsNotNull())
            {
                writer.WritePropertyName("facet_filter");
                serializer.Serialize(writer, facetRequest.FacetFilter);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(NestedHistogramFacetRequest).IsAssignableFrom(objectType);
        }
    }
}
