﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find;
﻿using EPiServer.Find.Api.Facets;
﻿using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.ClientConventions;
using FluentAssertions;
using StoryQ;
using Xunit;
using System.Threading;
using Nested2Find.ClientConventions;

namespace Nested2Find.Stories
{
    public class Nested
    {
        [Fact]
        public void FilterByMultipleValuesForAComplexObjectInAListUsingFilter()
        {
            new Story("Filter by matching a specific item in a list")
                .InOrderTo("be able to filter by several values on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and filter by multiple values in a unique object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldo)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamWithAPlayerWithFirstNameCristianoAndLastNameRonaldo)
                .Then(IShouldGetASingleHit)
                .And(ItShouldBeForTheFirstTeam)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldo()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo" });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoe()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe" });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldo()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo" });
        }

        void IHaveIndexedTheTeamObjects()
        {
            client.Index(team1, team2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Team> result;
        void ISearchForTeamWithAPlayerWithFirstNameCristianoAndLastNameRonaldo()
        {
            result = client.Search<Team>()
                        .Filter(x => x.Players, p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo"))
                        .GetResult();
        }

        void IShouldGetASingleHit()
        {
            result.TotalMatching.Should().Be(1);
        }

        void ItShouldBeForTheFirstTeam()
        {
            result.Single().TeamName.Should().Be(team1.TeamName);
        }
    }

    public class Nested2
    {
        [Fact]
        public void FilterByMultipleValuesForAComplexObjectInAListUsingFilterDelegateBuilder()
        {
            new Story("Filter by matching a specific item in a list")
                .InOrderTo("be able to filter by several values on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and filter by multiple values in a unique object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldo)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamWithAPlayerWithFirstNameCristianoAndLastNameRonaldo)
                .Then(IShouldGetASingleHit)
                .And(ItShouldBeForTheFirstTeam)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldo()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo" });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoe()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe" });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldo()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo" });
        }

        void IHaveIndexedTheTeamObjects()
        {
            client.Index(team1, team2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Team> result;
        void ISearchForTeamWithAPlayerWithFirstNameCristianoAndLastNameRonaldo()
        {
            result = client.Search<Team>()
                        .Filter(x => x.Players.MatchItem(p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo")))
                        .GetResult();
        }

        void IShouldGetASingleHit()
        {
            result.TotalMatching.Should().Be(1);
        }

        void ItShouldBeForTheFirstTeam()
        {
            result.Single().TeamName.Should().Be(team1.TeamName);
        }
    }

    public class Nested3
    {
        [Fact]
        public void FilterByMultipleValuesForANestedListOfComplexObjectsUsingFilterDelegateBuilder()
        {
            new Story("Filter by matching a specific item in a list")
                .InOrderTo("be able to filter by several values on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and filter by multiple values in a unique object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoLeagueObjects)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldo)
                    .And(TheFirstTeamPlaysInTheFirstLeague)
                    .And(TheSecondTeamPlaysInTheSecondLeague)
                    .And(IHaveIndexedTheLeagueObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForLeagugeWithTeamWithAPlayerWithFirstNameCristianoAndLastNameRonaldo)
                .Then(IShouldGetASingleHit)
                .And(ItShouldBeForTheFirstTeam)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private League league1, league2;
        void IHaveTwoLeagueObjects()
        {
            league1 = new League("League 1");
            league2 = new League("League 2");
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldo()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo" });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoe()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe" });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldo()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo" });
        }

        void TheFirstTeamPlaysInTheFirstLeague()
        {
            league1.Teams.Add(team1);
        }

        void TheSecondTeamPlaysInTheSecondLeague()
        {
            league2.Teams.Add(team2);
        }

        void IHaveIndexedTheLeagueObjects()
        {
            client.Index(league1, league2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<League> result;
        void ISearchForLeagugeWithTeamWithAPlayerWithFirstNameCristianoAndLastNameRonaldo()
        {
            result = client.Search<League>()
                        .Filter(x => x.Teams.MatchItem(t => t.Players.MatchItem(p => p.FirstName.Match("Cristiano") & p.LastName.Match("Ronaldo"))))
                        .GetResult();
        }

        void IShouldGetASingleHit()
        {
            result.TotalMatching.Should().Be(1);
        }

        void ItShouldBeForTheFirstTeam()
        {
            result.Single().LeagueName.Should().Be(league1.LeagueName);
        }
    }

    public class NestedTermFacet1
    {
        [Fact]
        public void RequestTermsFacetOnANestedListOfComplexObjects()
        {
            new Story("Reqest a terms facet on a item property in a list")
                .InOrderTo("be able to create a terms facet on a item property in a lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and request a terms facet on a item property")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldo)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamAndRequestATermsFacetOnPlayersFirstName)
                .Then(IShouldGetAResultWithATermsFacetForPlayersFirstName)
                    .And(ItShouldHaveTermChristianoWithCount2)
                    .And(ItShouldHaveTermJohnWithCount1)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldo()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo" });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoe()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe" });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldo()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo" });
        }

        void IHaveIndexedTheTeamObjects()
        {
            client.Index(team1, team2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Team> result;
        void ISearchForTeamAndRequestATermsFacetOnPlayersFirstName()
        {
            result = client.Search<Team>()
                        .TermsFacetFor(x => x.Players, x => x.FirstName)
                        .GetResult();
        }

        TermsFacet facet;
        void IShouldGetAResultWithATermsFacetForPlayersFirstName()
        {
            facet = result.TermsFacetFor(x => x.Players, x => x.FirstName);
            facet.Should().NotBeNull();
        }

        void ItShouldHaveTermChristianoWithCount2()
        {
            facet.Terms.First().Term.Should().Be("Cristiano");
            facet.Terms.First().Count.Should().Be(2);
        }

        void ItShouldHaveTermJohnWithCount1()
        {
            facet.Terms.ElementAt(1).Term.Should().Be("John");
            facet.Terms.ElementAt(1).Count.Should().Be(1);
        }
    }

    public class NestedTermFacet2
    {
        [Fact]
        public void RequestTermsFacetOnANestedListOfComplexObjectsInANestedListOfComplexObjects()
        {
            new Story("Reqest a terms facet on a item property in a list in a list of items")
                .InOrderTo("be able to create a terms facet on a item property in a lists of complex objects within a list of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and request a terms facet on a item property")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoLeagueObjects)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldo)
                    .And(TheFirstTeamPlaysInTheFirstLeague)
                    .And(TheSecondTeamPlaysInTheSecondLeague)
                    .And(IHaveIndexedTheLeagueObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForleagueAndRequestATermsFacetOnTeamsPlayersFirstName)
                .Then(IShouldGetAResultWithATermsFacetForTeamsPlayersFirstName)
                    .And(ItShouldHaveTermChristianoWithCount2)
                    .And(ItShouldHaveTermJohnWithCount1)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private League league1, league2;
        void IHaveTwoLeagueObjects()
        {
            league1 = new League("League 1");
            league2 = new League("League 2");
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldo()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo" });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoe()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe" });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldo()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo" });
        }

        void TheFirstTeamPlaysInTheFirstLeague()
        {
            league1.Teams.Add(team1);
        }

        void TheSecondTeamPlaysInTheSecondLeague()
        {
            league2.Teams.Add(team2);
        }

        void IHaveIndexedTheLeagueObjects()
        {
            client.Index(league1, league2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<League> result;
        void ISearchForleagueAndRequestATermsFacetOnTeamsPlayersFirstName()
        {
            result = client.Search<League>()
                        .TermsFacetFor(x => x.Teams, x => x.Players, x => x.FirstName)
                        .GetResult();
        }

        TermsFacet facet;
        void IShouldGetAResultWithATermsFacetForTeamsPlayersFirstName()
        {
            facet = result.TermsFacetFor(x => x.Teams, x => x.Players, x => x.FirstName);
            facet.Should().NotBeNull();
        }

        void ItShouldHaveTermChristianoWithCount2()
        {
            facet.Terms.First().Term.Should().Be("Cristiano");
            facet.Terms.First().Count.Should().Be(2);
        }

        void ItShouldHaveTermJohnWithCount1()
        {
            facet.Terms.ElementAt(1).Term.Should().Be("John");
            facet.Terms.ElementAt(1).Count.Should().Be(1);
        }
    }

    public class NestedHistogramFacet1
    {
        [Fact]
        public void RequestHistogramFacetOnANestedListOfComplexObjects()
        {
            new Story("Reqest a histogram facet on a item property in a list")
                .InOrderTo("be able to create a histogram facet on a item property in a lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and request a histogram facet on a item property")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary100000000)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary1000)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldoWithSalary10)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamAndRequestAHistogramFacetOnPlayersSalary)
                .Then(IShouldGetAResultWithAHistogramFacetForPlayersSalary)
                    .And(ItShouldHaveABucketFor10WithCount1)
                    .And(ItShouldHaveABucketFor1000WithCount1)
                    .And(ItShouldHaveABucketFor100000000WithCount1)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary100000000()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo", Salary = 100000000 });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary1000()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe", Salary = 1000 });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldoWithSalary10()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo", Salary = 10 });
        }

        void IHaveIndexedTheTeamObjects()
        {
            client.Index(team1, team2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Team> result;
        void ISearchForTeamAndRequestAHistogramFacetOnPlayersSalary()
        {
            result = client.Search<Team>()
                        .HistogramFacetFor(x => x.Players, x => x.Salary, 10)
                        .GetResult();
        }

        HistogramFacet facet;
        void IShouldGetAResultWithAHistogramFacetForPlayersSalary()
        {
            facet = result.HistogramFacetFor(x => x.Players, x => x.Salary);
            facet.Should().NotBeNull();
        }

        void ItShouldHaveABucketFor10WithCount1()
        {
            facet.Entries.Should().Contain(x => x.Key == 10 && x.Count == 1);
        }

        void ItShouldHaveABucketFor1000WithCount1()
        {
            facet.Entries.Should().Contain(x => x.Key == 1000 && x.Count == 1);
        }

        void ItShouldHaveABucketFor100000000WithCount1()
        {
            facet.Entries.Should().Contain(x => x.Key == 100000000 && x.Count == 1);
        }
    }

    public class NestedHistogramFacet2
    {
        [Fact]
        public void RequestHistogramFacetOnANestedListOfComplexObjects()
        {
            new Story("Reqest a histogram facet on a item property in a list")
                .InOrderTo("be able to create a histogram facet on a item property in a lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and request a histogram facet on a item property")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoLeagueObjects)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary100000000)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary1000)
                    .And(TheSecondTeamHasAPlayerNamedJohnRonaldoWithSalary10)
                    .And(TheFirstTeamPlaysInTheFirstLeague)
                    .And(TheSecondTeamPlaysInTheSecondLeague)
                    .And(IHaveIndexedTheLeagueObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForLeagueAndRequestAHistogramFacetOnTeamsPlayersSalary)
                .Then(IShouldGetAResultWithAHistogramFacetForTeamsPlayersSalary)
                    .And(ItShouldHaveABucketFor10WithCount1)
                    .And(ItShouldHaveABucketFor1000WithCount1)
                    .And(ItShouldHaveABucketFor100000000WithCount1)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedIEnumerablePropertiesAsNested()
        {
            client.Conventions.AddNestedConventions();
        }

        private League league1, league2;
        void IHaveTwoLeagueObjects()
        {
            league1 = new League("League 1");
            league2 = new League("League 2");
        }

        private Team team1, team2;
        void IHaveTwoTeamObjects()
        {
            team1 = new Team("Team 1");
            team2 = new Team("Team 2");
        }

        void TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary100000000()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo", Salary = 100000000 });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary1000()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe", Salary = 1000 });
        }

        void TheSecondTeamHasAPlayerNamedJohnRonaldoWithSalary10()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Ronaldo", Salary = 10 });
        }

        void TheFirstTeamPlaysInTheFirstLeague()
        {
            league1.Teams.Add(team1);
        }

        void TheSecondTeamPlaysInTheSecondLeague()
        {
            league2.Teams.Add(team2);
        }

        void IHaveIndexedTheLeagueObjects()
        {
            client.Index(league1, league2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<League> result;
        void ISearchForLeagueAndRequestAHistogramFacetOnTeamsPlayersSalary()
        {
            result = client.Search<League>()
                        .HistogramFacetFor(x => x.Teams, x => x.Players, x => x.Salary, 10)
                        .GetResult();
        }

        HistogramFacet facet;
        void IShouldGetAResultWithAHistogramFacetForTeamsPlayersSalary()
        {
            facet = result.HistogramFacetFor(x => x.Teams, x => x.Players, x => x.Salary);
            facet.Should().NotBeNull();
        }

        void ItShouldHaveABucketFor10WithCount1()
        {
            facet.Entries.Should().Contain(x => x.Key == 10 && x.Count == 1);
        }

        void ItShouldHaveABucketFor1000WithCount1()
        {
            facet.Entries.Should().Contain(x => x.Key == 1000 && x.Count == 1);
        }

        void ItShouldHaveABucketFor100000000WithCount1()
        {
            facet.Entries.Should().Contain(x => x.Key == 100000000 && x.Count == 1);
        }
    }

    public class League
    {
        public League(string name)
        {
            LeagueName = name;
            Teams = new NestedList<Team>();
        }
        public string LeagueName { get; set; }
        public NestedList<Team> Teams { get; set; }
    }

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
}
