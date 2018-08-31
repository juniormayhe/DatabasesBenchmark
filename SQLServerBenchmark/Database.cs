
using DatabaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace SQLServerBenchmark
{
    public class Database
    {
        const string connectionString = "Data Source=(local);Initial Catalog=DatabaseBenchmark;Integrated Security=True";

        static Random rnd = new Random();

        
        public static void Delete(ref BenchmarkResult br)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                new SqlCommand("DELETE FROM Employee", connection).ExecuteNonQuery();
                new SqlCommand("DELETE FROM Project", connection).ExecuteNonQuery();
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

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand($"SELECT Id, Name FROM Project ORDER BY Name", connection))
                {
                    connection.Open();
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lista.Add(new Project {
                                Id = Convert.ToInt32(dr["Id"]),
                                Name = Convert.ToString(dr["Name"])
                            });
                            
                        }
                    }
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

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand($"SELECT Id, Name, ProjectId FROM Employee ORDER BY Name", connection))
                {
                    connection.Open();
                    using (IDataReader dr = cmd.ExecuteReader())
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
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var item in d)
                {
                    new SqlCommand($"INSERT INTO Project (Id, Name) VALUES ({item.Key}, '{item.Value.Name}')", connection).ExecuteNonQuery();
                }
            }
        }

        private static void addEmployees(int total)
        {
            //generate random data
            Dictionary<int, Employee> d = getRandomEmployees(total);

            //insert data
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in d)
                {
                    new SqlCommand($"INSERT INTO Employee (Id, Name, ProjectId) VALUES ({item.Key}, '{item.Value.Name}', {item.Value.ProjectId})", connection).ExecuteNonQuery();
                }
            }

        }

        private static IEnumerable<Employee> getEmployeesByProject(int projectId)
        {
            var lista = new List<Employee>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand($"SELECT Id, Name, ProjectId FROM Employee WHERE ProjectId = {projectId} ORDER BY Name", connection))
                {
                    connection.Open();
                    using (IDataReader dr = cmd.ExecuteReader())
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

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand($"SELECT Id, Name FROM Project WHERE Id = {id}", connection))
                {
                    connection.Open();
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            p.Id = Convert.ToInt32(dr["Id"]);
                            p.Name = Convert.ToString(dr["Name"]);
                        }
                    }
                }

            }
            return p;
        }


    }
}
