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
                    if (type.GetGenericTypeDefinition() == typeof(NestedList<>))
                    {
                        fieldName += "$$nested";
                    }
                }
            }

            return fieldName;
        }
    }
}
