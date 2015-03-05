using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nested2Find.Tests.Support.Model
{
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
