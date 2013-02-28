using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using EPiServer.Find.Json;
using System.Linq.Expressions;

namespace Nested2Find.ClientConventions
{
    public class IncludeTypeNameInNestedListFieldNamesInterceptor : IInterceptObjectContract
    {
        public JsonObjectContract ModifyContract(JsonObjectContract contract)
        {
            foreach (var property in contract.Properties)
            {
                foreach (var type in property.PropertyType.GetInterfaces().Union(new Type[] { property.PropertyType }))
                {
                    if (type.IsGenericType)
                    {
                        if (type.GetGenericTypeDefinition() == typeof(NestedList<>))
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
