using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Helpers;
using Nested2Find.Api;
using System;
using System.Linq.Expressions;

namespace Nested2Find
{
    public static class NestedSortingExtensions
    {
        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem, TProperty>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, null);
        }

        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem, TProperty>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.First, SortOrder.Ascending);
        }

        public static ITypeSearch<TSource> OrderByDescending<TSource, TEnumerableItem, TProperty>(
           this ITypeSearch<TSource> search,
           Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, null);
        }

        public static ITypeSearch<TSource> OrderByDescending<TSource, TEnumerableItem, TProperty>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.Last, SortOrder.Descending);
        }

        public static ITypeSearch<TSource> ThenBy<TSource, TEnumerableItem, TProperty>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector)
        {
            return search.ThenBy(enumerableFieldSelector, itemFieldSelector, null);
        }

        public static ITypeSearch<TSource> ThenBy<TSource, TEnumerableItem, TProperty>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.First, SortOrder.Ascending);
        }

        public static ITypeSearch<TSource> ThenByDescending<TSource, TEnumerableItem, TProperty>(
           this ITypeSearch<TSource> search,
           Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector)
        {
            return search.ThenByDescending(enumerableFieldSelector, itemFieldSelector, null);
        }

        public static ITypeSearch<TSource> ThenByDescending<TSource, TEnumerableItem, TProperty>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.Last, SortOrder.Descending);
        }

        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem, TProperty>(
           this ITypeSearch<TSource> search,
           Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, TProperty>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression, SortMissing? sortMissing, SortOrder? order = null, SortMode? mode = null, bool? ignoreUnmapped = null)
        {
            enumerableFieldSelector.ValidateNotNullArgument("enumerableFieldSelector");
            itemFieldSelector.ValidateNotNullArgument("itemFieldSelector");

            var path = search.Client.Conventions.FieldNameConvention.GetFieldName(enumerableFieldSelector);

            Filter nestedFilter = null;
            if (filterExpression.IsNotNull())
            {
                var parser = new FilterExpressionParser(search.Client.Conventions);
                nestedFilter = parser.GetFilter(filterExpression);
                NestedFilterExtensions.PrependPathOnNestedFilters(path, nestedFilter);
            }

            return new Search<TSource, IQuery>(search, context =>
                context.RequestBody.Sort.Add(new NestedSorting(path + "." + search.Client.Conventions.FieldNameConvention.GetFieldNameForSort(itemFieldSelector))
                {
                    Missing = sortMissing,
                    Order = order,
                    Mode = mode,
                    IgnoreUnmapped = ignoreUnmapped,
                    NestedPath = path,
                    NestedFilter = nestedFilter
                }));
        }

        #region Int/DateTime
        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, int>> itemFieldSelector, SortMode? mode)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, null, mode);
        }

        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, int>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression, SortMode? mode)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.First, SortOrder.Ascending, mode);
        }

        public static ITypeSearch<TSource> OrderByDescending<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, int>> itemFieldSelector, SortMode? mode)
        {
            return search.OrderByDescending(enumerableFieldSelector, itemFieldSelector, null, mode);
        }

        public static ITypeSearch<TSource> OrderByDescending<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, int>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression, SortMode? mode)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.First, SortOrder.Descending, mode);
        }

        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, DateTime>> itemFieldSelector, SortMode? mode)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, null, mode);
        }

        public static ITypeSearch<TSource> OrderBy<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, DateTime>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression, SortMode? mode)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.First, SortOrder.Ascending, mode);
        }

        public static ITypeSearch<TSource> OrderByDescending<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, DateTime>> itemFieldSelector, SortMode? mode)
        {
            return search.OrderByDescending(enumerableFieldSelector, itemFieldSelector, null, mode);
        }

        public static ITypeSearch<TSource> OrderByDescending<TSource, TEnumerableItem>(
            this ITypeSearch<TSource> search,
            Expression<Func<TSource, NestedList<TEnumerableItem>>> enumerableFieldSelector, Expression<Func<TEnumerableItem, DateTime>> itemFieldSelector, Expression<Func<TEnumerableItem, Filter>> filterExpression, SortMode? mode)
        {
            return search.OrderBy(enumerableFieldSelector, itemFieldSelector, filterExpression, SortMissing.First, SortOrder.Descending, mode);
        }
        #endregion
    }
}
