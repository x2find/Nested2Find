using System;
using EPiServer.Find;
using EPiServer.Find.Helpers;
using EPiServer.Find.Json;

namespace Nested2Find.ClientConventions
{
    public static class NestedClientConventionsExtensions
    {
        public static void AddNestedConventions(this IClientConventions conventions)
        {
            conventions.ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInNestedListFieldNamesInterceptor());
            conventions.FieldNameConvention = new NestedFieldNameConvention();
        }
    }
}
