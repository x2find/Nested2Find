using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find;
using EPiServer.Find.Helpers;
using System.Linq.Expressions;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Helpers.Reflection;
using Nested2Find.Api.Querying.Filters;

namespace Nested2Find
{
    public static class NestedFilterExtensions
    {
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

        private static void PrependPathOnNestedFilters(string path, object filterOrQuery)
        {
            if (filterOrQuery is NestedFilter)
            {
                var nestedFilter = filterOrQuery as NestedFilter;
                nestedFilter.Path = string.Format("{0}.{1}", path, nestedFilter.Path);

                PrependPathOnNestedFilters(path, nestedFilter.Filter);
                return;
            }

            if (typeof(IEnumerable<Filter>).IsAssignableFrom(filterOrQuery.GetType()))
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
                if (obj is Filter || obj is IQuery)
                {
                    PrependPathOnNestedFilters(path, obj);
                }

                if (typeof(IEnumerable<Filter>).IsAssignableFrom(obj.GetType()))
                {
                    PrependPathOnNestedFilters(path, obj);
                }
            }

            return;
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
            var parser = new FilterExpressionParser(search.Client.Conventions);
            var filter = parser.GetFilter(filterExpression);
            var nestedFilter = new NestedFilter(path, filter);
            return search.Filter(nestedFilter);
        }
    }
}
