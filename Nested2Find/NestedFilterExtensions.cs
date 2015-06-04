using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;
using Nested2Find.Api.Querying.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nested2Find
{
    public static class NestedFilterExtensions
    {
        public static Filter ParseFilterExpression<TSource, TFilter>(ITypeSearch<TSource> search, Expression<Func<TFilter, Filter>> filterExpression)
        {
            var parser = new FilterExpressionParser(search.Client.Conventions);
            var filter = parser.GetFilter(filterExpression);
            return filter;
        }

        public static void PrependPathOnNestedFilters(string path, object filterOrQuery)
        {
            if (filterOrQuery is NestedFilter)
            {
                var nestedFilter = filterOrQuery as NestedFilter;
                nestedFilter.Path = string.Format("{0}.{1}", path, nestedFilter.Path);

                PrependPathOnNestedFilters(path, nestedFilter.Filter);
                return;
            }

            if (filterOrQuery is IEnumerable<Filter>)
            {
                var filterList = filterOrQuery as IEnumerable<Filter>;
                foreach (var filter in filterList)
                {
                    PrependPathOnNestedFilters(path, filter);
                }
                return;
            }

            foreach (var property in filterOrQuery.GetType().GetProperties())
            {
                var obj = property.GetGetMethod().Invoke(filterOrQuery, new object[] { });

                if (obj.IsNull())
                {
                    continue;
                }

                if (obj is Filter || obj is IQuery)
                {
                    PrependPathOnNestedFilters(path, obj);
                }

                if (obj is IEnumerable<Filter>)
                {
                    PrependPathOnNestedFilters(path, obj);
                }

                if (obj is String && property.Name.Equals("Field")) // prepend path to field name properties
                {
                    property.SetValue(filterOrQuery, string.Format("{0}.{1}", path, obj), null);
                }
            }

            return;
        }

        public static DelegateFilterBuilder MatchItem<TListItem>(this NestedList<TListItem> value,
                                                           Expression<Func<TListItem, Filter>> filterExpression)
        {
            return new DelegateExpressionFilterBuilder<TListItem>(filterExpression, filter => x =>
            {
                // As the path must be absolute nested NestedFilters must have the path prepended
                PrependPathOnNestedFilters(x, filter);

                return new NestedFilter(x, filter);
            });
        }

        public static ITypeSearch<TSource> Filter<TSource, TListItem>(this ITypeSearch<TSource> search,
                                                                  Expression<Func<TSource, NestedList<TListItem>>> nestedExpression,
                                                                  Expression<Func<TListItem, Filter>> filterExpression)
        {
            search.ValidateNotNullArgument("search");
            nestedExpression.ValidateNotNullArgument("nestedExpression");
            filterExpression.ValidateNotNullArgument("filterExpression");

            var path = search.Client.Conventions.FieldNameConvention.GetFieldName(nestedExpression);
            if (!path.EndsWith("$$nested"))
            {
                throw new ArgumentException(string.Format("{0} is not a nested object.", nestedExpression.GetReturnType().Name));
            }
            var filter = ParseFilterExpression(search, filterExpression);
            PrependPathOnNestedFilters(path, filter);
            var nestedFilter = new NestedFilter(path, filter);
            return search.Filter(nestedFilter);
        }
    }
}
