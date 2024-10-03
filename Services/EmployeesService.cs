using InterviewTest.DTOs;
using InterviewTest.Model;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InterviewTest.Services
{
    public class EmployeesService
    {
        public EmployeesService() { }

        public List<Employee> GetAll()
        {
            var employees = new List<Employee>();

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees";

                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        });
                    }
                }
            }

            return employees;
        }

        public Employee FindByValue(int value)
        {
            Employee employee = null;

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees WHERE Value = @Value";
                queryCmd.Parameters.AddWithValue("@Value", value);

                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employee = new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        };
                    }
                }
            }

            if (employee is null) return null;

            return employee;
        }

        public Employee FindByName(string name)
        {
            Employee employee = null;

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees WHERE Name = @Name";
                queryCmd.Parameters.AddWithValue("@Name", name);

                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employee = new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        };
                    }
                }
            }

            if (employee is null) return null;

            return employee;
        }

        public Employee Create(Employee employee)
        {
            int numberOfRowsUpdated = 0;

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"INSERT INTO Employees (Name, Value) VALUES (@Name, @Value)";
                queryCmd.Parameters.AddWithValue("@Name", employee.Name);
                queryCmd.Parameters.AddWithValue("@Value", employee.Value);

                numberOfRowsUpdated = queryCmd.ExecuteNonQuery();
            }

            if (numberOfRowsUpdated == 0) return null;

            return employee;
        }

        public void FindByNameAndUpdate(string name, Employee employeeToUpdate)
        {
            int numberOfRowsUpdated = 0;

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"
                    UPDATE Employees 
                    SET Name = @NewName, Value = @NewValue WHERE Name = @Name";
                queryCmd.Parameters.AddWithValue("@NewName", employeeToUpdate.Name);
                queryCmd.Parameters.AddWithValue("@NewValue", employeeToUpdate.Value);
                queryCmd.Parameters.AddWithValue("@Name", name);

                numberOfRowsUpdated = queryCmd.ExecuteNonQuery();
            }

            if (numberOfRowsUpdated == 0)
                throw new Exception("Error while updating entry");
        }

        public void FindByNameAndRemove(string name)
        {
            int numberOfRowsUpdated = 0;

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"DELETE FROM Employees WHERE Name = @Name";
                queryCmd.Parameters.AddWithValue("@Name", name);

                numberOfRowsUpdated = queryCmd.ExecuteNonQuery();
            }

            if (numberOfRowsUpdated == 0)
                throw new Exception("Error while deleting from database");
        }

        public SumQueryResponse ExecuteSumQuery()
        {
            SumQueryResponse sumQueryResult = null;

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"
                    SELECT 
                        CASE WHEN SUM(CASE WHEN SUBSTR(Name, 1, 1) = 'A' THEN Value END) >= 11171 THEN SUM(CASE WHEN SUBSTR(Name, 1, 1) = 'A' THEN Value END) ELSE -1 END AS SumOfA,
                        CASE WHEN SUM(CASE WHEN SUBSTR(Name, 1, 1) = 'B' THEN Value END) >= 11171 THEN SUM(CASE WHEN SUBSTR(Name, 1, 1) = 'B' THEN Value END) ELSE -1 END AS SumOfB,
                        CASE WHEN SUM(CASE WHEN SUBSTR(Name, 1, 1) = 'C' THEN Value END) >= 11171 THEN SUM(CASE WHEN SUBSTR(Name, 1, 1) = 'C' THEN Value END) ELSE -1 END AS SumOfC
                    FROM Employees;";

                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sumQueryResult = new SumQueryResponse
                        (
                            SumOfA: reader.GetInt32(0),
                            SumOfB: reader.GetInt32(1),
                            SumOfC: reader.GetInt32(2)
                        );
                    }
                }
            }

            return sumQueryResult;
        }

        public void ExecuteIncrementQuery()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"
                    UPDATE Employees 
                    SET Value = Value + CASE
                        WHEN SUBSTR(Name, 1, 1) = 'E' THEN 1
                        WHEN SUBSTR(Name, 1, 1) = 'G' THEN 10
                        ELSE 100
                    END";

                queryCmd.ExecuteNonQuery();
            }
        }
    }
}
