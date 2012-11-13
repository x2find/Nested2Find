using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Helpers.Reflection;
using System.Linq.Expressions;
using System.Collections;

namespace Nested2Find.ClientConventions
{
    public class NestedFieldNameConvention : TypeSuffixFieldNameConvention
    {
        public override string GetFieldName(Expression fieldSelector)
        {
            var fieldName = base.GetFieldName(fieldSelector);

            foreach (var type in fieldSelector.GetReturnType().GetInterfaces().Union(new Type[] { fieldSelector.GetReturnType() }))
            {
                if (type.IsGenericType)
                {
                    // we want to nest only lists of objects not previously mapped to a specific type by EPiServer Find
                    if ((type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        && !(type.GetGenericArguments()[0].IsValueType || type.GetGenericArguments()[0] == typeof(string) || type.GetGenericArguments()[0] == typeof(DateTime))
                        && !fieldName.Contains("$$")
                        && !fieldName.StartsWith("__"))
                    {
                        fieldName += "$$nested";
                    }
                }
            }

            return fieldName;
        }
    }
}
