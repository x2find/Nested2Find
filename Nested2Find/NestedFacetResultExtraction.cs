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
    public static class NestedFacetResultExtraction
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

        #region TermsFacet
        public static TermsFacet TermsFacetFor<TResult, TEnumerableItem>(
            this IHasFacetResults<TResult> facetsResultsContainer, Expression<Func<TResult, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, string>> itemFieldSelector)
        {
            return GetFacetFor<TermsFacet>(facetsResultsContainer, enumerableFieldSelector.GetFieldPath() + "." + itemFieldSelector.GetFieldPath());
        }
        #endregion

        #region HistogramFacet
        public static HistogramFacet HistogramFacetFor<TResult, TEnumerableItem>(
            this IHasFacetResults<TResult> facetsResultsContainer, Expression<Func<TResult, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, object>> itemFieldSelector)
        {
            return GetFacetFor<HistogramFacet>(facetsResultsContainer, enumerableFieldSelector.GetFieldPath() + "." + itemFieldSelector.GetFieldPath());
        }
        #endregion

        #region DateHistogramFacet
        public static DateHistogramFacet DateHistogramFacetFor<TResult, TEnumerableItem>(
            this IHasFacetResults<TResult> facetsResultsContainer, Expression<Func<TResult, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, DateTime?>> itemFieldSelector)
        {
            return GetFacetFor<DateHistogramFacet>(facetsResultsContainer, enumerableFieldSelector.GetFieldPath() + "." + itemFieldSelector.GetFieldPath());
        }
        #endregion
    }
}
