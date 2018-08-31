using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModel
{
    public class BenchmarkResult
    {
        public BenchmarkResult(string name)
        {
            DatabaseName = name;
        }
        public string DatabaseName { get; set; }
        public double GetEmployeesElapsedMilliseconds { get; set; }
        public double GetEmployeesByProjectElapsedMilliseconds { get; set; }
        public double GetProjectsElapsedMilliseconds { get; set; }
        public double GetProjectsByIdElapsedMilliseconds { get; set; }
        public double InsertProjectsElapsedMilliseconds { get; set; }
        public double InsertEmployeesElapsedMilliseconds { get; set; }
        public double DeleteElapsedMilliseconds { get; set; }

        public override string ToString()
        {
            return $"{InsertProjectsElapsedMilliseconds/1000.0}\t\t{InsertEmployeesElapsedMilliseconds / 1000.0}\t\t{GetProjectsElapsedMilliseconds / 1000.0}\t\t{GetProjectsByIdElapsedMilliseconds / 1000.0}\t\t{GetEmployeesElapsedMilliseconds / 1000.0}\t\t{GetEmployeesByProjectElapsedMilliseconds / 1000.0}\t\t\t\t{DeleteElapsedMilliseconds / 1000.0}";
        }
    }
}
