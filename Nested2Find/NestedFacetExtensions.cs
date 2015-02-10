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
using Nested2Find.Helpers.Reflection;

namespace Nested2Find
{
    public static class NestedFacetExtensions
    {
        #region TermsFacet
        public static ITypeSearch<TSource> TermsFacetFor<TSource, TEnumerableItem>(
        this ITypeSearch<TSource> search,
        Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, string>> itemFieldSelector)
        {
            return search.TermsFacetFor(enumerableFieldSelector, itemFieldSelector, null);
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
                facetRequest.Field = search.Client.Conventions.FieldNameConvention.GetFieldName(itemFieldSelector);
                facetRequest.Nested = enumerableFieldSelector.GetNestedFieldPath();
                if (action.IsNotNull())
                {
                    action(facetRequest);
                }
                context.RequestBody.Facets.Add(facetRequest);
            });
        }

        public static ITypeSearch<TSource> TermsFacetFor<TSource, TEnumerableItem1, TEnumerableItem2>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem1>>> enumerableFieldSelector1, Expression<Func<TEnumerableItem1, NestedList<TEnumerableItem2>>> enumerableFieldSelector2, Expression<Func<TEnumerableItem2, string>> itemFieldSelector)
        {
            return search.TermsFacetFor(enumerableFieldSelector1, enumerableFieldSelector2, itemFieldSelector, null);
        }

        public static ITypeSearch<TSource> TermsFacetFor<TSource, TEnumerableItem1, TEnumerableItem2>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem1>>> enumerableFieldSelector1, Expression<Func<TEnumerableItem1, NestedList<TEnumerableItem2>>> enumerableFieldSelector2, Expression<Func<TEnumerableItem2, string>> itemFieldSelector, Action<NestedTermsFacetRequest> facetRequestAction)
        {
            return search.AddNestedTermsFacetFor(enumerableFieldSelector1, enumerableFieldSelector2, itemFieldSelector, facetRequestAction);
        }

        private static ITypeSearch<TSource> AddNestedTermsFacetFor<TSource>(
            this ITypeSearch<TSource> search,
            Expression enumerableFieldSelector1, Expression enumerableFieldSelector2, Expression itemFieldSelector, Action<NestedTermsFacetRequest> facetRequestAction)
        {
            enumerableFieldSelector1.ValidateNotNullArgument("enumerableFieldSelector1");
            enumerableFieldSelector2.ValidateNotNullArgument("enumerableFieldSelector2");
            itemFieldSelector.ValidateNotNullArgument("itemFieldSelector");

            var facetName = enumerableFieldSelector1.GetFieldPath() + "." + enumerableFieldSelector2.GetFieldPath() + "." + itemFieldSelector.GetFieldPath();
            var action = facetRequestAction;
            return new Search<TSource, IQuery>(search, context =>
            {
                var facetRequest = new NestedTermsFacetRequest(facetName);
                facetRequest.Field = search.Client.Conventions.FieldNameConvention.GetFieldName(itemFieldSelector);
                facetRequest.Nested = enumerableFieldSelector1.GetNestedFieldPath() + "." + enumerableFieldSelector2.GetNestedFieldPath();
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
