﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
﻿using EPiServer.Find.Api.Querying.Queries;
﻿using EPiServer.Find.ClientConventions;
﻿using EPiServer.Find.Helpers.Text;
﻿using FluentAssertions;
using StoryQ;
using Xunit;
using System.Threading;
using Nested2Find.ClientConventions;

namespace Nested2Find.Stories
{
    public class Nested : IDisposable
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


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Nested2 : IDisposable
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

        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Nested3 : IDisposable
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

        public void Dispose()
        {
            client.Delete<League>(x => x.LeagueName.Match(league1.LeagueName));
            client.Delete<League>(x => x.LeagueName.Match(league1.LeagueName));
        }
    }

    public class Nested4 : IDisposable
    {
        [Fact]
        public void FilterByMultipleValuesForANestedListOfComplexObjectsWithDupplicatePropertyNamesUsingFilterDelegateBuilder()
        {
            new Story("Filter by matching a specific item in a list")
                .InOrderTo("be able to filter by several values on objects in lists of complex objects having similar property names")
                .AsA("developer")
                .IWant("to be able to map list of complex objects as nested and filter by multiple values in a unique object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoCompanyObjects)
                    .And(IHaveTwoDepartmentObjects)
                    .And(TheFirstDepartmentHasAnEmployeeNamedCristianoRonaldo)
                    .And(TheSecondDepartmentHasAnEmployeeNamedCristianoDoe)
                    .And(TheSecondDepartmentHasAnEmployeeNamedJohnRonaldo)
                    .And(TheFirstDepartmentBelongsToTheFirstCompany)
                    .And(TheSecondDepartmentBelongsToTheSecondCompany)
                    .And(IHaveIndexedTheCompanyObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForCompanyWithDepartmentWithAnEmployeeWithNameCristianoAndLastNameRonaldo)
                .Then(IShouldGetASingleHit)
                .And(ItShouldBeForTheFirstDepartment)
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

        private Company company1, company2;
        void IHaveTwoCompanyObjects()
        {
            company1 = new Company("Company 1");
            company2 = new Company("Company 2");
        }

        private Department department1, department2;
        void IHaveTwoDepartmentObjects()
        {
            department1 = new Department("Department 1");
            department2 = new Department("Department 2");
        }

        void TheFirstDepartmentHasAnEmployeeNamedCristianoRonaldo()
        {
            department1.Employees.Add(new Employee { Name = "Cristiano", LastName = "Ronaldo" });
        }

        void TheSecondDepartmentHasAnEmployeeNamedCristianoDoe()
        {
            department2.Employees.Add(new Employee { Name = "Cristiano", LastName = "Doe" });
        }

        void TheSecondDepartmentHasAnEmployeeNamedJohnRonaldo()
        {
            department2.Employees.Add(new Employee { Name = "John", LastName = "Ronaldo" });
        }

        void TheFirstDepartmentBelongsToTheFirstCompany()
        {
            company1.Departments.Add(department1);
        }

        void TheSecondDepartmentBelongsToTheSecondCompany()
        {
            company2.Departments.Add(department2);
        }

        void IHaveIndexedTheCompanyObjects()
        {
            client.Index(company1, company2);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Company> result;
        void ISearchForCompanyWithDepartmentWithAnEmployeeWithNameCristianoAndLastNameRonaldo()
        {
            result = client.Search<Company>()
                        .Filter(x => x.Departments.MatchItem(t => t.Employees.MatchItem(p => p.Name.Match("Cristiano") & p.LastName.Match("Ronaldo"))))
                        .GetResult();
        }

        void IShouldGetASingleHit()
        {
            result.TotalMatching.Should().Be(1);
        }

        void ItShouldBeForTheFirstDepartment()
        {
            result.Single().Name.Should().Be(company1.Name);
        }

        public void Dispose()
        {
            client.Delete<Company>(x => x.Name.Match(company1.Name));
            client.Delete<Company>(x => x.Name.Match(company2.Name));
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
    }

    public class Company
    {
        public Company(string name)
        {
            Name = name;
            Departments = new NestedList<Department>();
        }

        public string Name { get; set; }
        public NestedList<Department> Departments { get; set; } 
    }

    public class Department
    {
        public Department(string name)
        {
            Name = name;
            Employees = new NestedList<Employee>();
        }

        public string Name { get; set; }
        public NestedList<Employee> Employees { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string LastName { get; set; }

    }
}
