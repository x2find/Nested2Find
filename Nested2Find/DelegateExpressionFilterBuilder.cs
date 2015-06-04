using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using System;
using System.Linq.Expressions;

namespace Nested2Find
{
    public class DelegateExpressionFilterBuilder<TSource> : DelegateFilterBuilder
    {
        private Func<string, Filter> filterDelegate;

        public DelegateExpressionFilterBuilder(Expression<Func<TSource, Filter>> filterExpression, Func<Filter, Func<string, Filter>> expressionFilterDelegate) : base(null)
        {
            FieldNameMethod = (expression, conventions) => {
                // highjack fieldname method to parse expression since we now know the conventions
                var parser = new FilterExpressionParser(conventions);
                var filter = parser.GetFilter(filterExpression);
                filterDelegate = expressionFilterDelegate(filter);
                
                return conventions.FieldNameConvention.GetFieldName(expression);
            };
        }

        public override Filter GetFilter(string fieldName)
        {
            return filterDelegate(fieldName);
        }
    }
}
