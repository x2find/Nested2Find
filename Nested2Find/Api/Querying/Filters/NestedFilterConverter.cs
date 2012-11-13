using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using EPiServer.Find.Helpers;

namespace Nested2Find.Api.Querying.Filters
{
    class NestedFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.IsNot<NestedFilter>())
            {
                writer.WriteNull();
                return;
            }
            var filter = (NestedFilter)value;
            writer.WriteStartObject();
            writer.WritePropertyName("nested");
            writer.WriteStartObject();
            if (filter.Path.IsNotNull())
            {
                writer.WritePropertyName("path");
                writer.WriteValue(filter.Path);
            }
            writer.WritePropertyName("filter");
            serializer.Serialize(writer, filter.Filter);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(NestedFilter).IsAssignableFrom(objectType);
        }
    }
}
