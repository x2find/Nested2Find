using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;
using Nested2Find.Api.Facets;
using Nested2Find.Api.Querying.Filters;
using Nested2Find.Helpers.Reflection;

namespace Nested2Find
{
    public static class NestedFacetExtensions
    {
        #region TermsFacet
        public static ITypeSearch<TSource> TermsFacetFor<TSource, TEnumerableItem>(
        this ITypeSearch<TSource> search,
        Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, string>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression = null)
        {
            Filter facetFilter = null;
            if (filterExpression.IsNotNull())
            {
                var path = search.Client.Conventions.FieldNameConvention.GetFieldName(enumerableFieldSelector);
                facetFilter = NestedFilterExtensions.ParseFilterExpression(search, filterExpression);
                NestedFilterExtensions.PrependPathOnNestedFilters(path, facetFilter);
                facetFilter = new NestedFilter(search.Client.Conventions.FieldNameConvention.GetFieldName(enumerableFieldSelector), facetFilter)
                {
                    Join = false
                };
            }
            return search.TermsFacetFor(enumerableFieldSelector, itemFieldSelector,  x => x.FacetFilter = facetFilter);
        }

        public static ITypeSearch<TSource> TermsFacetFor<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, string>> itemFieldSelector, Action<NestedTermsFacetRequest> facetRequestAction)
        {
            return search.AddNestedTermsFacetFor(enumerableFieldSelector, itemFieldSelector, facetRequestAction);
        }

        private static ITypeSearch<TSource> AddNestedTermsFacetFor<TSource>(
            this ITypeSearch<TSource> search,
            Expression enumerableFieldSelector, Expression itemFieldSelector, Action<NestedTermsFacetRequest> facetRequestAction)
        {
            enumerableFieldSelector.ValidateNotNullArgument("enumerableFieldSelector");
            itemFieldSelector.ValidateNotNullArgument("itemFieldSelector");

            var facetName = enumerableFieldSelector.GetFieldPath() + "." + itemFieldSelector.GetFieldPath();
            var action = facetRequestAction;
            return new Search<TSource, IQuery>(search, context =>
            {
                var facetRequest = new NestedTermsFacetRequest(facetName);
                facetRequest.Field = enumerableFieldSelector.GetNestedFieldPath() + "." + search.Client.Conventions.FieldNameConvention.GetFieldName(itemFieldSelector);
                facetRequest.Nested = enumerableFieldSelector.GetNestedFieldPath();
                if (action.IsNotNull())
                {
                    action(facetRequest);
                }
                context.RequestBody.Facets.Add(facetRequest);
            });
        }
        #endregion

        #region HistogramFacet
        public static ITypeSearch<TSource> HistogramFacetFor<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, object>> itemFieldSelector, int interval, Expression<Func<TEnumerableItem, Filter>> filterExpression = null)
        {
            Filter facetFilter = null;
            if (filterExpression.IsNotNull())
            {
                var path = search.Client.Conventions.FieldNameConvention.GetFieldName(enumerableFieldSelector);
                facetFilter = NestedFilterExtensions.ParseFilterExpression(search, filterExpression);
                NestedFilterExtensions.PrependPathOnNestedFilters(path, facetFilter);
                facetFilter = new NestedFilter(search.Client.Conventions.FieldNameConvention.GetFieldName(enumerableFieldSelector), facetFilter)
                {
                    Join = false
                };
            }
            return search.AddNestedHistogramFacetFor(enumerableFieldSelector, itemFieldSelector, x =>
            {
                x.Interval = interval;
                x.FacetFilter = facetFilter;
            });
        }

        private static ITypeSearch<TSource> AddNestedHistogramFacetFor<TSource>(
            this ITypeSearch<TSource> search,
            Expression enumerableFieldSelector, Expression itemFieldSelector, Action<NestedHistogramFacetRequest> facetRequestAction)
        {
            enumerableFieldSelector.ValidateNotNullArgument("enumerableFieldSelector");
            itemFieldSelector.ValidateNotNullArgument("itemFieldSelector");

            var facetName = enumerableFieldSelector.GetFieldPath() + "." + itemFieldSelector.GetFieldPath();
            var action = facetRequestAction;
            return new Search<TSource, IQuery>(search, context =>
            {
                var facetRequest = new NestedHistogramFacetRequest(facetName);
                facetRequest.Field = enumerableFieldSelector.GetNestedFieldPath() + "." + search.Client.Conventions.FieldNameConvention.GetFieldName(itemFieldSelector);
                facetRequest.Nested = enumerableFieldSelector.GetNestedFieldPath();
                if (action.IsNotNull())
                {
                    action(facetRequest);
                }
                context.RequestBody.Facets.Add(facetRequest);
            });
        }
        #endregion
    }
}
