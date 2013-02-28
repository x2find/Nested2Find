using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using EPiServer.Find;
using Nested2Find;
using Nested2Find.ClientConventions;

namespace Nested2Find.Tests.Specs
{
    public class when_filtering_on_a_list_of_non_nested_objects
    {
        static IClient client;
        static Exception exception;

        Establish context = () =>
        {
            client = Client.CreateFromConfig();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => client.Search<Document>().Filter(x => x.Authors, s => s.Name.Match("abc")));
        };

        It should_have_thrown_an_exception = () => 
            exception.ShouldNotBeNull();
    }
}
