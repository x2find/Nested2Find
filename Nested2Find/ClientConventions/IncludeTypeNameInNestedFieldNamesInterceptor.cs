using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using EPiServer.Find.Json;

namespace Nested2Find.ClientConventions
{
    public class IncludeTypeNameInNestedFieldNamesInterceptor : IInterceptObjectContract
    {
        public JsonObjectContract ModifyContract(JsonObjectContract contract)
        {
            foreach (var property in contract.Properties)
            {
                foreach (var type in property.PropertyType.GetInterfaces().Union(new Type[] { property.PropertyType }))
                {
                    if (type.IsGenericType)
                    {
                        // we want to nest only lists of objects not previously mapped to a specific type by EPiServer Find
                        if ((type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                            && !(type.GetGenericArguments()[0].IsValueType || type.GetGenericArguments()[0] == typeof(string) || type.GetGenericArguments()[0] == typeof(DateTime))
                            && !property.PropertyName.Contains("$$") 
                            && !property.PropertyName.StartsWith("__"))
                        {
                            property.PropertyName = property.PropertyName + "$$nested";
                        }
                    }
                }
            }

            return contract;
        }
    }
}
