using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nested2Find.Tests.Support.Model
{
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
