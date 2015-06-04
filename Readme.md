Nested2Find
===========

Adds nested filtering to EPiServer Find's .NET API

### Build

In order to build Nested2Find the NuGet packages that it depends on must be restored.
See http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages

### Usage

Add the nested conventions to the conventions:

```c#
client.Conventions.AddNestedConventions();
```

Create an object containing a NestedList of objects

```c#
public class Team
{
    public Team(string name)
    {
        TeamName = name;
        Players = new NestedList<Player>();
    }
    public string TeamName { get; set; }
    public NestedList<Player> Players { get; set; }
}

public class Player
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Salary { get; set; }
}
```

Index and start filtering:

```c#
result = client.Search<Team>()
             .Filter(x => x.Players, p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo"))
             .GetResult();
```

or:

```c#
result = client.Search<Team>()
            .Filter(x => x.Players.MatchItem(p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo")))
            .GetResult();
```

#### Sorting

In order to sort on nested properties the OrderBy-extensions takes a filter argument that specifies on which nested object the sort value should be calculated:

```c#
result = client.Search<Team>()
            .Filter(x => x.Players, p => p.FirstName.Match("Cristiano"))
            .OrderBy(x => x.Players, p => p.LastName, p => p.FirstName.Match("Cristiano"))
            .GetResult();
```

or:

```c#
result = client.Search<Team>()
            .Filter(x => x.Players, p => p.FirstName.Match("Cristiano"))
            .OrderByDescending(x => x.Players, p => p.LastName, p => p.FirstName.Match("Cristiano"))
            .GetResult();
```

As for int/DateTime a SortMode (Min/Max/Avg/Sum) can be specified to decide how to treat multiple sort values. To sort by the mamimum player salary in a team use:

```c#
result = client.Search<Team>()
            .OrderByDescending(x => x.Players, p => p.Salary, SortMode.Max)
            .GetResult();
```

#### Facets

In order to facet on nested properties TermsFacetFor/HistogramFacetFor/DateHistogramFacetFor-extensions takes a NestedList expression along with an item expression and an optional filter:

```c#
result = client.Search<Team>()
            .TermsFacetFor(x => x.Players, x => x.FirstName)
            .GetResult();
```

or (to filter):

```c#
result = client.Search<Team>()
            .TermsFacetFor(x => x.Players, x => x.FirstName, x => x.LastName.Match("Ronaldo"))
            .GetResult();
```

and to fetch the result:

```c#
facet = result.TermsFacetFor(x => x.Players, x => x.FirstName);
```