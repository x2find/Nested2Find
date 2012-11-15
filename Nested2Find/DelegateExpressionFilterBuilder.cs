using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find;
using System.Linq.Expressions;
using EPiServer.Find.Api.Querying;

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
