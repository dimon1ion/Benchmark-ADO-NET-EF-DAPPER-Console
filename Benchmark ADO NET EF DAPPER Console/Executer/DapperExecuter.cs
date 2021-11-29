using Benchmark_ADO_NET_EF_DAPPER_Console.EFModels;
using Benchmark_ADO_NET_EF_DAPPER_Console.Executer.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Benchmark_ADO_NET_EF_DAPPER_Console.Executer
{
    public class DapperExecuter : IDbExecuter
    {
        public string ConnectionString { get; set; }
        public DataTable Data { get; set; }

        public DapperExecuter(string connectionString)
        {
            ConnectionString = connectionString;
            Data = new DataTable();
            Data.Columns.Add("Resolved");
            Data.Columns.Add("Resolver");
            Data.Columns.Add("Name");
            Data.Columns.Add("Timing");
        }

        private void AddRow(DataTable data, string Resolved, string Resolver, string Name, string Timing)
        {
            DataRow row = data.NewRow();
            row[nameof(Resolved)] = Resolved;
            row[nameof(Resolver)] = Resolver;
            row[nameof(Name)] = Name;
            row[nameof(Timing)] = Timing;
            data.Rows.Add(row);
        }

        public DataTable AddMillionRecords()
        {
            Data.Clear();
            Random rnd = new Random();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Human";
                connection.Execute(query);
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    query = "INSERT INTO Human(FirstName, LastName, Age, Gender) VALUES(@FirstName, @LastName, @Age, @Gender);";
                    for (int i = 0; i < 100000; i++)
                    {
                        connection.Execute(query, new { FirstName = $"FirstName{i}", LastName = $"LastName{i}", Age = rnd.Next(5, 26), Gender = rnd.Next(0, 2) == 1 });
                    }
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "AddMillionRecords", result.timing.ToString());
                }

            }

            return Data;
        }

        public DataTable AddOneRecord()
        {
            Data.Clear();
            Random rnd = new Random();
            string query;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    query = "INSERT INTO Human(FirstName, LastName, Age, Gender) VALUES(@FirstName, @LastName, @Age, @Gender);";
                    connection.Execute(query, new { FirstName = $"FirstName0{rnd.Next(0, 100)}", LastName = $"LastName0{rnd.Next(0, 100)}", Age = rnd.Next(5, 26), Gender = rnd.Next(0, 2) == 1 });
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "AddOneRecord", result.timing.ToString());
                }

            }
            return Data;
        }

        public DataTable GetOneRecord()
        {
            Data.Clear();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.QuerySingle<int>("Select COUNT(Id) FROM Human") == 0)
                {
                    AddOneRecord();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Execute("Select TOP 1 * FROM Human");
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "GetOneRecord", result.timing.ToString());
                }
            }
            return Data;
        }

        public DataTable GetAllRecords()
        {
            Data.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.QuerySingle<int>("Select COUNT(Id) FROM Human") < 90000)
                {
                    AddMillionRecords();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Query<Human>("Select * FROM Human");
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "GetAllRecords", result.timing.ToString());
                }
            }

            return Data;
        }

        public DataTable FindFirstNameColumn(string value)
        {
            Data.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Query<Human>("SELECT * FROM Human WHERE FirstName LIKE'%@FirstName%'", new { FirstName = value });
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "FindFirstNameColumn", result.timing.ToString());
                }

                return Data;
            }
        }

        public DataTable UpdateOneRecord()
        {
            Data.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.QuerySingle<int>("Select COUNT(Id) FROM Human") == 0)
                {
                    AddOneRecord();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Execute("UPDATE Human SET FirstName = 'FirstName' WHERE Id = (SELECT TOP 1 Id FROM Human)");
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "UpdateOneRecord", result.timing.ToString());
                }

            }

            return Data;
        }

        public DataTable UpdateAllRecords()
        {
            Data.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.QuerySingle<int>("Select COUNT(Id) FROM Human") < 90000)
                {
                    AddMillionRecords();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Execute("UPDATE Human SET LastName = 'LastName'");
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "UpdateAllRecords", result.timing.ToString());
                }

            }

            return Data;
        }

        public DataTable DeleteOneRecord()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.QuerySingle<int>("Select COUNT(Id) FROM Human") == 0)
                {
                    AddOneRecord();
                }
                Data.Clear();
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Execute("Delete TOP(1) FROM Human");
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "DeleteOneRecord", result.timing.ToString());
                }

            }

            return Data;
        }

        public DataTable DeleteAllRecord()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.QuerySingle<int>("Select COUNT(Id) FROM Human") < 90000)
                {
                    AddMillionRecords();
                }
                Data.Clear();
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    connection.Execute("Delete FROM Human");
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Dapper", "DeleteAllRecord", result.timing.ToString());
                }

            }

            return Data;
        }
    }
}