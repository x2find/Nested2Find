using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find.Api;
using EPiServer.Find.Helpers;
using Newtonsoft.Json;

namespace Nested2Find.Api
{
    public class NestedSortingConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.IsNot<NestedSorting>())
            {
                writer.WriteNull();
                return;
            }

            var sorting = (NestedSorting) value;
            writer.WriteStartObject();
            writer.WritePropertyName(sorting.FieldName);
            writer.WriteStartObject();

            if (sorting.Order.HasValue)
            {
                writer.WritePropertyName("order");
                if (sorting.Order.Value == SortOrder.Ascending)
                {
                    writer.WriteValue("asc");
                }
                else
                {
                    writer.WriteValue("desc");
                }
            }

            if (sorting.Missing.HasValue)
            {
                writer.WritePropertyName("missing");
                if (sorting.Missing.Value == SortMissing.First)
                {
                    writer.WriteValue("_first");
                }
                else
                {
                    writer.WriteValue("_last");
                }
            }

            if (sorting.Mode.HasValue)
            {
                writer.WritePropertyName("mode");
                writer.WriteValue(sorting.Mode.ToString().ToLower());
            }

            if (sorting.IgnoreUnmapped.HasValue)
            {
                writer.WritePropertyName("ignore_unmapped");
                writer.WriteValue(sorting.IgnoreUnmapped.Value);
            }

            if (sorting.NestedPath.IsNotNull())
            {
                writer.WritePropertyName("nested_path");
                serializer.Serialize(writer, sorting.NestedPath);
            }

            if (sorting.NestedFilter.IsNotNull())
            {
                writer.WritePropertyName("nested_filter");
                serializer.Serialize(writer, sorting.NestedFilter);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (NestedSorting).IsAssignableFrom(objectType);
        }
    }
}
