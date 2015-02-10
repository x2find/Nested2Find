using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;

namespace Nested2Find
{
    public static class NestedTermsFacetResultExtraction
    {
        private static TFacet GetFacetFor<TFacet>(IHasFacetResults facetsResultsContainer, string facetName)
            where TFacet : Facet
        {
            var facet = facetsResultsContainer.Facets[facetName];
            if (facet.IsNull())
            {
                return null;
            }

            return facet as TFacet;
        }

        public static TermsFacet TermsFacetFor<TResult, TEnumerableItem>(
            this IHasFacetResults<TResult> facetsResultsContainer, Expression<Func<TResult, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, string>> itemFieldSelector)
        {
            return GetFacetFor<TermsFacet>(facetsResultsContainer, enumerableFieldSelector.GetFieldPath() + "." + itemFieldSelector.GetFieldPath());
        }

        public static TermsFacet TermsFacetFor<TResult, TEnumerableItem1, TEnumerableItem2>(
            this IHasFacetResults<TResult> facetsResultsContainer, Expression<Func<TResult, NestedList<TEnumerableItem1>>> enumerableFieldSelector1, Expression<Func<TEnumerableItem1, NestedList<TEnumerableItem2>>> enumerableFieldSelector2, Expression<Func<TEnumerableItem2, string>> itemFieldSelector)
        {
            return GetFacetFor<TermsFacet>(facetsResultsContainer, enumerableFieldSelector1.GetFieldPath() + "." + enumerableFieldSelector2.GetFieldPath() + "." + itemFieldSelector.GetFieldPath());
        }
    }
}
