using DatabaseModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DatabaseBenchmark { 

    class Program {

        static void Main(string[] args)
        {
            benchmarkNuoDB();
            benchmarkSQLServer();
            

            WriteLine("Ok");
            ReadKey();
        }

        private static void benchmarkSQLServer()
        {
            var currentForeground = ForegroundColor;
            ForegroundColor = ConsoleColor.Green;
            WriteLine("------------------------");
            WriteLine("Benchmark for SQL Server");
            WriteLine("------------------------");
            ForegroundColor = currentForeground;
            BenchmarkResult br = new BenchmarkResult("SQLServer");
            SQLServerBenchmark.Database.Delete(ref br);
            WriteLine("InsProjects\tInsEmployees\tGetProjects\tGetProjectsById\tGetEmployees\tGetEmployeesByProject\tDelete");
            var list = new List<BenchmarkResult>();
            for (int i = 0; i < 10; i++)
            {
                br = new BenchmarkResult("SQLServer");
                SQLServerBenchmark.Database.InsertProjects(ref br);
                SQLServerBenchmark.Database.InsertEmployees(ref br);
                SQLServerBenchmark.Database.GetProjects(ref br);
                SQLServerBenchmark.Database.GetProjectsById(ref br);
                SQLServerBenchmark.Database.GetEmployees(ref br);
                SQLServerBenchmark.Database.GetEmployeesByProject(ref br);
                SQLServerBenchmark.Database.Delete(ref br);
                WriteLine(br.ToString());
                list.Add(br);
            }
            br = new BenchmarkResult("SQLServer");

            br.InsertProjectsElapsedMilliseconds = list.Average(x => x.InsertProjectsElapsedMilliseconds);
            br.InsertEmployeesElapsedMilliseconds = list.Average(x => x.InsertEmployeesElapsedMilliseconds);

            br.GetProjectsElapsedMilliseconds = list.Average(x => x.GetProjectsElapsedMilliseconds);
            br.GetProjectsByIdElapsedMilliseconds = list.Average(x => x.GetProjectsByIdElapsedMilliseconds);

            br.GetEmployeesByProjectElapsedMilliseconds = list.Average(x => x.GetEmployeesByProjectElapsedMilliseconds);
            br.GetEmployeesElapsedMilliseconds = list.Average(x => x.GetEmployeesElapsedMilliseconds);

            br.DeleteElapsedMilliseconds = list.Average(x => x.DeleteElapsedMilliseconds);

            ForegroundColor = ConsoleColor.Green;

            WriteLine("------------------------");
            ForegroundColor = currentForeground;
            WriteLine(br.ToString());
        }

        private static void benchmarkNuoDB()
        {
            var currentForeground = ForegroundColor;
            ForegroundColor = ConsoleColor.Green;
            WriteLine("------------------------");
            WriteLine("Benchmark for NuoDB");
            WriteLine("------------------------");
            ForegroundColor = currentForeground;
            BenchmarkResult br = new BenchmarkResult("NuoDB");
            NuoDBBenchmark.Database.Delete(ref br);
            WriteLine("InsProjects\tInsEmployees\tGetProjects\tGetProjectsById\tGetEmployees\tGetEmployeesByProject\tDelete");
            var list = new List<BenchmarkResult>();
            for (int i = 0; i < 10; i++)
            {
                br = new BenchmarkResult("NuoDB");
                NuoDBBenchmark.Database.InsertProjects(ref br);
                NuoDBBenchmark.Database.InsertEmployees(ref br);
                NuoDBBenchmark.Database.GetProjects(ref br);
                NuoDBBenchmark.Database.GetProjectsById(ref br);
                NuoDBBenchmark.Database.GetEmployees(ref br);
                NuoDBBenchmark.Database.GetEmployeesByProject(ref br);
                NuoDBBenchmark.Database.Delete(ref br);
                WriteLine(br.ToString());
                list.Add(br);
            }
            br = new BenchmarkResult("NuoDB");

            br.InsertProjectsElapsedMilliseconds = list.Average(x => x.InsertProjectsElapsedMilliseconds);
            br.InsertEmployeesElapsedMilliseconds = list.Average(x => x.InsertEmployeesElapsedMilliseconds);

            br.GetProjectsElapsedMilliseconds = list.Average(x => x.GetProjectsElapsedMilliseconds);
            br.GetProjectsByIdElapsedMilliseconds = list.Average(x => x.GetProjectsByIdElapsedMilliseconds);

            br.GetEmployeesByProjectElapsedMilliseconds = list.Average(x => x.GetEmployeesByProjectElapsedMilliseconds);
            br.GetEmployeesElapsedMilliseconds = list.Average(x => x.GetEmployeesElapsedMilliseconds);

            br.DeleteElapsedMilliseconds = list.Average(x => x.DeleteElapsedMilliseconds);

            ForegroundColor = ConsoleColor.Green;

            WriteLine("------------------------");
            ForegroundColor = currentForeground;
            WriteLine(br.ToString());
        }
    }


}