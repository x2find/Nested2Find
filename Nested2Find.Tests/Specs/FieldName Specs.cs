using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using EPiServer.Find;
using Nested2Find;
using Nested2Find.ClientConventions;
using System.Linq.Expressions;

namespace Nested2Find.Tests.Specs
{
    public class when_getting_the_field_name_of_a_nested_list
    {
        static NestedFieldNameConvention fieldNameConvention;
        static string fieldName;

        Establish context = () =>
        {
            fieldNameConvention = new NestedFieldNameConvention();
        };

        Because of = () =>
        {
            Expression<Func<Document, NestedList<Author>>> expression = d => d.Authors;
            fieldName = fieldNameConvention.GetFieldName(expression);
        };

        It should_be_suffixed_nested = () =>
            fieldName.ShouldContain("$$nested");
    }

    public class when_getting_the_field_name_of_a_non_nested_list
    {
        static NestedFieldNameConvention fieldNameConvention;
        static string fieldName;

        Establish context = () =>
        {
            fieldNameConvention = new NestedFieldNameConvention();
        };

        Because of = () =>
        {
            Expression<Func<Document, IEnumerable<string>>> expression = d => d.Tags;
            fieldName = fieldNameConvention.GetFieldName(expression);
        };

        It should_not_be_suffixed_nested = () =>
            fieldName.ShouldNotContain("$$nested");
    }

    public class when_getting_the_field_name_of_a_complex_object
    {
        static NestedFieldNameConvention fieldNameConvention;
        static string fieldName;

        Establish context = () =>
        {
            fieldNameConvention = new NestedFieldNameConvention();
        };

        Because of = () =>
        {
            Expression<Func<Document, Author>> expression = d => d.MainAuthor;
            fieldName = fieldNameConvention.GetFieldName(expression);
        };

        It should_not_be_suffixed_nested = () =>
            fieldName.ShouldNotContain("$$nested");
    }

    class Document
    {
        public string Title { get; set; }

        public Author MainAuthor { get; set; }

        public NestedList<Author> Authors { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    class Author
    {
        public string Name { get; set; }
    }
}
