
using DatabaseModel;
using NuoDb.Data;
using NuoDb.Data.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace NuoDBBenchmark
{
    public class Database
    {
        static string connectionString = "Server=localhost;Database=DatabaseBenchmark;User=sa;Password=testando;Schema=HOCKEY";

        static Random rnd = new Random();
        static NuoDbConnectionStringBuilder builder;
        static Database(){

            //builder = new NuoDbConnectionStringBuilder();
            //builder.Server = "localhost";
            //builder.Database = "DatabaseBenchmark";
            //builder.User = "sa";
            //builder.Password = "testando";
            //builder.Schema = "HOCKEY";
            //connectionString = ";
            
        }
        
        public static void Delete(ref BenchmarkResult br)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            using (var connection = new NuoDbConnection(connectionString))
            {
                connection.Open();
                new NuoDbCommand("DELETE FROM Employee", connection).ExecuteNonQuery();
                new NuoDbCommand("DELETE FROM Project", connection).ExecuteNonQuery();
                connection.Close();
            }
            watch.Stop();
            br.DeleteElapsedMilliseconds = watch.ElapsedMilliseconds;
        }

        public static void InsertEmployees(ref BenchmarkResult br)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            addEmployees(10000);
            
            watch.Stop();
            br.InsertEmployeesElapsedMilliseconds = watch.ElapsedMilliseconds;
        }

        public static void InsertProjects(ref BenchmarkResult br)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            addProjects(10000);

            watch.Stop();
            br.InsertProjectsElapsedMilliseconds = watch.ElapsedMilliseconds;
            
        }

        public static void GetProjectsById(ref BenchmarkResult br)
        {
            var lista = new List<int>();
            for (int i = 1; i <= 10000; i++) {
                lista.Add(i);
            }
            var result = lista.OrderBy(item => rnd.Next());
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 1; i <= 10000; i++)
            {
                getProjectById(lista[i-1]);
            }
            watch.Stop();
            br.GetProjectsByIdElapsedMilliseconds = watch.ElapsedMilliseconds;
            
        }

        public static IEnumerable<Project> GetProjects(ref BenchmarkResult br)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var lista = new List<Project>();

            using (var connection = new NuoDbConnection(connectionString))
            {
                using (var cmd = new NuoDbCommand($"SELECT Id, Name FROM Project ORDER BY Name", connection))
                {
                    connection.Open();
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lista.Add(new Project {
                                Id = Convert.ToInt32(dr["Id"]),
                                Name = Convert.ToString(dr["Name"])
                            });
                            
                        }
                    }
                    connection.Close();
                }
            }
            watch.Stop();
            br.GetProjectsElapsedMilliseconds = watch.ElapsedMilliseconds;
            

            return lista;
        }

        public static void GetEmployeesByProject(ref BenchmarkResult br)
        {
            var lista = new List<int>();
            for (int i = 1; i <= 10000; i++)
            {
                lista.Add(i-1);
            }
            var result = lista.OrderBy(item => rnd.Next());
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 1; i <= 10000; i++)
            {
                getEmployeesByProject(lista[i-1]);
            }
            watch.Stop();
            br.GetEmployeesByProjectElapsedMilliseconds = watch.ElapsedMilliseconds;
        }

        public static IEnumerable<Employee> GetEmployees(ref BenchmarkResult br)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var lista = new List<Employee>();

            using (var connection = new NuoDbConnection(connectionString))
            {
                using (var cmd = new NuoDbCommand($"SELECT Id, Name, ProjectId FROM Employee ORDER BY Name", connection))
                {
                    connection.Open();
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lista.Add(new Employee
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Name = Convert.ToString(dr["Name"]),
                                ProjectId = Convert.ToInt32(dr["ProjectId"])
                            });
                        }
                    }
                    connection.Close();
                }
            }
            watch.Stop();
            br.GetEmployeesElapsedMilliseconds = watch.ElapsedMilliseconds;
            
            return lista;
        }

        private static Dictionary<int, Project> getRandomProjects(int total)
        {
            var d = new Dictionary<int, Project>();
            for (int i = 1; i <= total; i++)
            {
                d.Add(i, new Project { Id = i, Name = randomString(10) });
            }

            return d;
        }

        private static Dictionary<int, Employee> getRandomEmployees(int total)
        {
            var d = new Dictionary<int, Employee>();
            for (int i = 1; i <= total; i++)
            {
                d.Add(i, new Employee { Id = i, Name= randomString(10), ProjectId = rnd.Next(1, total+1) });
            }

            return d;
        }

        private static void addProjects(int total)
        {
            //generate random data
            Dictionary<int, Project> d = getRandomProjects(total);

            //insert data
            using (var connection = new NuoDbConnection(connectionString))
            {
                connection.Open();
                foreach (var item in d)
                {
                    new NuoDbCommand($"INSERT INTO Project (Id, Name) VALUES ({item.Key}, '{item.Value.Name}')", connection).ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private static void addEmployees(int total)
        {
            //generate random data
            Dictionary<int, Employee> d = getRandomEmployees(total);

            //insert data
            using (var connection = new NuoDbConnection(connectionString))
            {
                connection.Open();

                foreach (var item in d)
                {
                    new NuoDbCommand($"INSERT INTO Employee (Id, Name, ProjectId) VALUES ({item.Key}, '{item.Value.Name}', {item.Value.ProjectId})", connection).ExecuteNonQuery();
                }
                connection.Close();
            }

        }

        private static IEnumerable<Employee> getEmployeesByProject(int projectId)
        {
            var lista = new List<Employee>();

            using (var connection = new NuoDbConnection(connectionString))
            {
                using (var cmd = new NuoDbCommand($"SELECT Id, Name, ProjectId FROM Employee WHERE ProjectId = {projectId} ORDER BY Name", connection))
                {
                    connection.Open();
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lista.Add(new Employee
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Name = Convert.ToString(dr["Name"]),
                                ProjectId = projectId
                            });

                        }
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        private static string randomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz ";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[rnd.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        private static Project getProjectById(int id)
        {
            Project p = new Project();

            using (var connection = new NuoDbConnection(connectionString))
            {
                using (var cmd = new NuoDbCommand($"SELECT Id, Name FROM Project WHERE Id = {id}", connection))
                {
                    connection.Open();
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            p.Id = Convert.ToInt32(dr["Id"]);
                            p.Name = Convert.ToString(dr["Name"]);
                        }
                    }
                    connection.Close();
                }

            }
            return p;
        }


    }
}
