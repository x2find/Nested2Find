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
        public void FilterByMultipleValuesForAComplexObjectInAList()
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
            result.Single().Name.Should().Be(team1.Name);
        }
    }

    public class Team
    {
        public Team(string name)
        {
            Name = name;
            Players = new List<Player>();
        }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
    }

    public class Player
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
