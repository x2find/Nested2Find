using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Helpers.Reflection;

namespace Nested2Find.Helpers.Reflection
{
    public static class ExpressionExtensions
    {
        public static string GetNestedFieldPath(this Expression fieldSelector)
        {
            string fieldPath = fieldSelector.GetFieldPath();
            foreach (var type in fieldSelector.GetReturnType().GetInterfaces().Union(new Type[] { fieldSelector.GetReturnType() }))
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(NestedList<>))
                    {
                        fieldPath += "$$nested";
                    }
                }
            }
            return fieldPath;
        }
    }
}
