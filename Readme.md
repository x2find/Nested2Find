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