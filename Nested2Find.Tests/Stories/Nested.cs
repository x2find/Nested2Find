﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
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
            client.Conventions.ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInNestedFieldNamesInterceptor());
            client.Conventions.FieldNameConvention = new NestedFieldNameConvention();
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
            client.Conventions.ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInNestedFieldNamesInterceptor());
            client.Conventions.FieldNameConvention = new NestedFieldNameConvention();
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
            client.Conventions.ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInNestedFieldNamesInterceptor());
            client.Conventions.FieldNameConvention = new NestedFieldNameConvention();
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

    public class League
    {
        public League(string name)
        {
            LeagueName = name;
            Teams = new List<Team>();
        }
        public string LeagueName { get; set; }
        public List<Team> Teams { get; set; }
    }

    public class Team
    {
        public Team(string name)
        {
            TeamName = name;
            Players = new List<Player>();
        }
        public string TeamName { get; set; }
        public List<Player> Players { get; set; }
    }

    public class Player
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
