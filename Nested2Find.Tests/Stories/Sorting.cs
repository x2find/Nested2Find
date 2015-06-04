using System;
using System.Linq;
using System.Threading;
using EPiServer.Find;
using FluentAssertions;
using Machine.Specifications;
using Nested2Find.Api;
using Nested2Find.ClientConventions;
using StoryQ;
using Xunit;
using Nested2Find.Tests.Support.Model;

namespace Nested2Find.Tests.Stories
{
    public class Sorting1 : IDisposable
    {
        [Fact]
        public void SortByStringValueInAListOfComplexObjects()
        {
            new Story("Sort by string value in a lists of complex objects")
                .InOrderTo("be able to sort by string value on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map a list of complex objects as nested and sort by a string value in an object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnDoe)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamWithAPlayerWithFirstNameCristianoAndSortByPlayersLastName)
                .Then(IShouldGetAListOfAllTeamsHavingAPlayerWithFirstNameRonaldo)
                .And(ItShouldBeSortedByThePlayersNamedRonaldosLastName)
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

        void TheSecondTeamHasAPlayerNamedJohnDoe()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Doe" });
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
        void ISearchForTeamWithAPlayerWithFirstNameCristianoAndSortByPlayersLastName()
        {
            result = client.Search<Team>()
                        .Filter(x => x.Players, p => p.FirstName.Match("Cristiano"))
                        .OrderBy(x => x.Players, p => p.LastName, p => p.FirstName.Match("Cristiano"))
                        .GetResult();
        }

        void IShouldGetAListOfAllTeamsHavingAPlayerWithFirstNameRonaldo()
        {
            result.TotalMatching.Should().Be(2);
            result.ShouldEachConformTo(t => t.Players.Exists(p => p.FirstName.Equals("Cristiano")));
        }

        void ItShouldBeSortedByThePlayersNamedRonaldosLastName()
        {
            result.ElementAt(0).ShouldMatch(t => t.TeamName.Equals("Team 2"));
            result.ElementAt(1).ShouldMatch(t => t.TeamName.Equals("Team 1"));
        }


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Sorting2 : IDisposable
    {
        [Fact]
        public void SortByStringValueOrderByDescendingInAListOfComplexObjects()
        {
            new Story("Sort by string value and order by descending in a lists of complex objects")
                .InOrderTo("be able to sort by string value and order by descending on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map a list of complex objects as nested and sort by a string value and order by descending in an object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldo)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoe)
                    .And(TheSecondTeamHasAPlayerNamedJohnDoe)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamWithAPlayerWithFirstNameCristianoAndSortByPlayersLastNameOrderByDescending)
                .Then(IShouldGetAListOfAllTeamsHavingAPlayerWithFirstNameRonaldo)
                .And(ItShouldBeSortedByThePlayersNamedRonaldosLastName)
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

        void TheSecondTeamHasAPlayerNamedJohnDoe()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Doe" });
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
        void ISearchForTeamWithAPlayerWithFirstNameCristianoAndSortByPlayersLastNameOrderByDescending()
        {
            result = client.Search<Team>()
                        .Filter(x => x.Players, p => p.FirstName.Match("Cristiano"))
                        .OrderByDescending(x => x.Players, p => p.LastName, p => p.FirstName.Match("Cristiano"))
                        .GetResult();
        }

        void IShouldGetAListOfAllTeamsHavingAPlayerWithFirstNameRonaldo()
        {
            result.TotalMatching.Should().Be(2);
            result.ShouldEachConformTo(t => t.Players.Exists(p => p.FirstName.Equals("Cristiano")));
        }

        void ItShouldBeSortedByThePlayersNamedRonaldosLastName()
        {
            result.ElementAt(0).ShouldMatch(t => t.TeamName.Equals("Team 1"));
            result.ElementAt(1).ShouldMatch(t => t.TeamName.Equals("Team 2"));
        }


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Sorting3 : IDisposable
    {
        [Fact]
        public void SortByIntegerMaxValueInAListOfComplexObjects()
        {
            new Story("Sort by integer maximum value in a lists of complex objects")
                .InOrderTo("be able to sort by integer maximum value on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map a list of complex objects as nested and sort by the integer maximum value in an object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary1000000)
                    .And(TheFirstTeamHasAPlayerNamedTheWaterboyWithSalary10)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary100000)
                    .And(TheSecondTeamHasAPlayerNamedJohnDoeWithSalary10000)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamsSortByMaximumPlayerSalary)
                .Then(IShouldGetAListOfAllTeams)
                .And(ItShouldBeSortedByTheMaximumPlayerSalary)
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

        void TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary1000000()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo", Salary = 1000000 });
        }

        void TheFirstTeamHasAPlayerNamedTheWaterboyWithSalary10()
        {
            team1.Players.Add(new Player { FirstName = "The", LastName = "Waterboy", Salary = 10 });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary100000()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe", Salary = 100000 });
        }

        void TheSecondTeamHasAPlayerNamedJohnDoeWithSalary10000()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Doe", Salary = 10000 });
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
        void ISearchForTeamsSortByMaximumPlayerSalary()
        {
            result = client.Search<Team>()
                        .OrderByDescending(x => x.Players, p => p.Salary, SortMode.Max)
                        .GetResult();
        }

        void IShouldGetAListOfAllTeams()
        {
            result.TotalMatching.Should().Be(2);
        }

        void ItShouldBeSortedByTheMaximumPlayerSalary()
        {
            result.ElementAt(0).ShouldMatch(t => t.TeamName.Equals("Team 1"));
            result.ElementAt(1).ShouldMatch(t => t.TeamName.Equals("Team 2"));
        }


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Sorting4 : IDisposable
    {
        [Fact]
        public void SortByIntegerMinimumValueInAListOfComplexObjects()
        {
            new Story("Sort by integer minimum value in a lists of complex objects")
                .InOrderTo("be able to sort by integer minimum value on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map a list of complex objects as nested and sort by the integer minimum value in an object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary1000000)
                    .And(TheFirstTeamHasAPlayerNamedTheWaterboyWithSalary10)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary100000)
                    .And(TheSecondTeamHasAPlayerNamedJohnDoeWithSalary10000)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamsSortByMinimumPlayerSalary)
                .Then(IShouldGetAListOfAllTeams)
                .And(ItShouldBeSortedByTheMinimumPlayerSalary)
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

        void TheFirstTeamHasAPlayerNamedCristianoRonaldoWithSalary1000000()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo", Salary = 1000000 });
        }

        void TheFirstTeamHasAPlayerNamedTheWaterboyWithSalary10()
        {
            team1.Players.Add(new Player { FirstName = "The", LastName = "Waterboy", Salary = 10 });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoeWithSalary100000()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe", Salary = 100000 });
        }

        void TheSecondTeamHasAPlayerNamedJohnDoeWithSalary10000()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Doe", Salary = 10000 });
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
        void ISearchForTeamsSortByMinimumPlayerSalary()
        {
            result = client.Search<Team>()
                        .OrderByDescending(x => x.Players, p => p.Salary, SortMode.Min)
                        .GetResult();
        }

        void IShouldGetAListOfAllTeams()
        {
            result.TotalMatching.Should().Be(2);
        }

        void ItShouldBeSortedByTheMinimumPlayerSalary()
        {
            result.ElementAt(0).ShouldMatch(t => t.TeamName.Equals("Team 2"));
            result.ElementAt(1).ShouldMatch(t => t.TeamName.Equals("Team 1"));
        }


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Sorting5 : IDisposable
    {
        [Fact]
        public void SortByMostRecentDateTimeValueInAListOfComplexObjects()
        {
            new Story("Sort by most recent datetime value in a lists of complex objects")
                .InOrderTo("be able to sort by most recent datetime value on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map a list of complex objects as nested and sort by the most recent datetime value in an object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldoSigned2009)
                    .And(TheFirstTeamHasAPlayerNamedTheWaterboySigned1998)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoeSigned2007)
                    .And(TheSecondTeamHasAPlayerNamedJohnDoeSigned2003)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamsSortByMostRecentSignDate)
                .Then(IShouldGetAListOfAllTeams)
                .And(ItShouldBeSortedByTheMostRecentSignDate)
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

        void TheFirstTeamHasAPlayerNamedCristianoRonaldoSigned2009()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo", SignDate = new DateTime(2009, 1, 1) });
        }

        void TheFirstTeamHasAPlayerNamedTheWaterboySigned1998()
        {
            team1.Players.Add(new Player { FirstName = "The", LastName = "Waterboy", SignDate = new DateTime(1998, 1, 1) });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoeSigned2007()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe", SignDate = new DateTime(2007, 1, 1) });
        }

        void TheSecondTeamHasAPlayerNamedJohnDoeSigned2003()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Doe", SignDate = new DateTime(2003, 1, 1) });
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
        void ISearchForTeamsSortByMostRecentSignDate()
        {
            result = client.Search<Team>()
                        .OrderByDescending(x => x.Players, p => p.SignDate, SortMode.Max)
                        .GetResult();
        }

        void IShouldGetAListOfAllTeams()
        {
            result.TotalMatching.Should().Be(2);
        }

        void ItShouldBeSortedByTheMostRecentSignDate()
        {
            result.ElementAt(0).ShouldMatch(t => t.TeamName.Equals("Team 1"));
            result.ElementAt(1).ShouldMatch(t => t.TeamName.Equals("Team 2"));
        }


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }

    public class Sorting6 : IDisposable
    {
        [Fact]
        public void SortByOldesttDateTimeValueInAListOfComplexObjects()
        {
            new Story("Sort by oldest datetime value in a lists of complex objects")
                .InOrderTo("be able to sort by oldest datetime value on objects in lists of complex objects")
                .AsA("developer")
                .IWant("to be able to map a list of complex objects as nested and sort by the oldest datetime value in an object in the list")
                .WithScenario("mapping list of complex objects as nested")
                .Given(IHaveAClient)
                    .And(IHaveMappedIEnumerablePropertiesAsNested)
                    .And(IHaveTwoTeamObjects)
                    .And(TheFirstTeamHasAPlayerNamedCristianoRonaldoSigned2009)
                    .And(TheFirstTeamHasAPlayerNamedTheWaterboySigned1998)
                    .And(TheSecondTeamHasAPlayerNamedCristianoDoeSigned2007)
                    .And(TheSecondTeamHasAPlayerNamedJohnDoeSigned2003)
                    .And(IHaveIndexedTheTeamObjects)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForTeamsSortByOldestSignDate)
                .Then(IShouldGetAListOfAllTeams)
                .And(ItShouldBeSortedByTheOldestSignDate)
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

        void TheFirstTeamHasAPlayerNamedCristianoRonaldoSigned2009()
        {
            team1.Players.Add(new Player { FirstName = "Cristiano", LastName = "Ronaldo", SignDate = new DateTime(2009, 1, 1) });
        }

        void TheFirstTeamHasAPlayerNamedTheWaterboySigned1998()
        {
            team1.Players.Add(new Player { FirstName = "The", LastName = "Waterboy", SignDate = new DateTime(1998, 1, 1) });
        }

        void TheSecondTeamHasAPlayerNamedCristianoDoeSigned2007()
        {
            team2.Players.Add(new Player { FirstName = "Cristiano", LastName = "Doe", SignDate = new DateTime(2007, 1, 1) });
        }

        void TheSecondTeamHasAPlayerNamedJohnDoeSigned2003()
        {
            team2.Players.Add(new Player { FirstName = "John", LastName = "Doe", SignDate = new DateTime(2003, 1, 1) });
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
        void ISearchForTeamsSortByOldestSignDate()
        {
            result = client.Search<Team>()
                        .OrderByDescending(x => x.Players, p => p.SignDate, SortMode.Min)
                        .GetResult();
        }

        void IShouldGetAListOfAllTeams()
        {
            result.TotalMatching.Should().Be(2);
        }

        void ItShouldBeSortedByTheOldestSignDate()
        {
            result.ElementAt(0).ShouldMatch(t => t.TeamName.Equals("Team 2"));
            result.ElementAt(1).ShouldMatch(t => t.TeamName.Equals("Team 1"));
        }


        public void Dispose()
        {
            client.Delete<Team>(x => x.TeamName.Match(team1.TeamName));
            client.Delete<Team>(x => x.TeamName.Match(team2.TeamName));
        }
    }
}
