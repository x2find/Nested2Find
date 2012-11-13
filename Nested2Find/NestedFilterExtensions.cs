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
        public static ITypeSearch<TSource> Filter<TSource, TListItem>(this ITypeSearch<TSource> search,
                                                                  Expression<Func<TSource, IEnumerable<TListItem>>> nestedExpression,
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
