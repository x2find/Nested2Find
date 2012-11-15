Nested2Find
===========

Adds nested filtering to EPiServer Find's .NET API

### Build

In order to build Nested2Find the NuGet packages that it depends on must be restored.
See http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages

### Usage

Include the IncludeTypeNameInNestedFieldNamesInterceptor and NestedFieldNameConvention to the conventions:

'''c#
client.Conventions.ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInNestedFieldNamesInterceptor());
client.Conventions.FieldNameConvention = new NestedFieldNameConvention();
'''

and start filtering:

'''c#
result = client.Search<Team>()
             .Filter(x => x.Players, p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo"))
             .GetResult();
'''

or:

'''c#
result = client.Search<Team>()
            .Filter(x => x.Players.MatchItem(p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo")))
            .GetResult();
'''