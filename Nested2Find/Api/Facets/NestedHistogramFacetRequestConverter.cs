using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Text;
using Newtonsoft.Json;

namespace Nested2Find.Api.Facets
{
    public class NestedHistogramFacetRequestConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var facetRequest = (NestedHistogramFacetRequest)value;
            writer.WriteStartObject();
            writer.WritePropertyName("histogram");
            writer.WriteStartObject();
            if (facetRequest.Field.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("field");
                writer.WriteValue(facetRequest.Field);
            }
            if (facetRequest.KeyField.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("key_field");
                writer.WriteValue(facetRequest.KeyField);
            }
            if (facetRequest.ValueField.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("value_field");
                writer.WriteValue(facetRequest.ValueField);
            }
            if (facetRequest.KeyScript.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("key_script");
                writer.WriteValue(facetRequest.KeyScript);
            }
            if (facetRequest.ValueScript.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("value_script");
                writer.WriteValue(facetRequest.ValueScript);
            }
            if (facetRequest.Interval.IsNotNull())
            {
                writer.WritePropertyName("interval");
                writer.WriteValue(facetRequest.Interval.Value);
            }
            writer.WriteEndObject();
            if (facetRequest.Nested.IsNotNullOrEmpty())
            {
                writer.WritePropertyName("nested");
                writer.WriteValue(facetRequest.Nested);
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
