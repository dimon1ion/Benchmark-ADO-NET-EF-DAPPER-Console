using Benchmark_ADO_NET_EF_DAPPER_Console.Executer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Benchmark_ADO_NET_EF_DAPPER_Console.Executer
{
    public class AdoNetExecuter : IDbExecuter
    {
        public string ConnectionString { get; set; }
        public DataTable Data { get; set; }

        public AdoNetExecuter(string connectionString)
        {
            this.ConnectionString = connectionString;
            Data = new DataTable();
            Data.Columns.Add("Resolved");
            Data.Columns.Add("Resolver");
            Data.Columns.Add("Name");
            Data.Columns.Add("Timing");
        }

        private DataTable AddRow(DataTable data, string Resolved, string Resolver, string Name, string Timing)
        {
            DataRow row = data.NewRow();
            row[nameof(Resolved)] = Resolved;
            row[nameof(Resolver)] = Resolver;
            row[nameof(Name)] = Name;
            row[nameof(Timing)] = Timing;
            data.Rows.Add(row);
            return data;
        }

        public DataTable AddMillionRecords()
        {
            Data.Clear();
            string query;
            SqlCommand command;
            Random rnd = new Random();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                query = "DELETE FROM Human";
                command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();

                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    query = "INSERT INTO Human(FirstName, LastName, Age, Gender) VALUES(@FirstName, @LastName, @Age, @Gender);";
                    for (int i = 1; i <= 100000; i++)
                    {
                        command = new SqlCommand(query, connection);
                        command.Parameters.Add(new SqlParameter("@FirstName", $"FirstName{i}"));
                        command.Parameters.Add(new SqlParameter("@LastName", $"LastName{i}"));
                        command.Parameters.Add(new SqlParameter("@Age", rnd.Next(5, 26)));
                        command.Parameters.Add(new SqlParameter("@Gender", rnd.Next(0, 2) == 1));
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "AddMillionRecords", result.timing.ToString());
                }

            }
            return Data;
        }

        public DataTable AddOneRecord()
        {
            Data.Clear();
            string query;
            SqlCommand command;
            Random rnd = new Random();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {


                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    query = "INSERT INTO Human(FirstName, LastName, Age, Gender) VALUES(@FirstName, @LastName, @Age, @Gender);";
                    command = new SqlCommand(query, connection);
                    command.Parameters.Add(new SqlParameter("@FirstName", $"FirstName0{rnd.Next(0, 100)}"));
                    command.Parameters.Add(new SqlParameter("@LastName", $"LastName0{rnd.Next(0, 100)}"));
                    command.Parameters.Add(new SqlParameter("@Age", rnd.Next(5, 26)));
                    command.Parameters.Add(new SqlParameter("@Gender", rnd.Next(0, 2) == 1));
                    command.ExecuteNonQuery();
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "AddOneRecord", result.timing.ToString());
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
                SqlCommand command1 = new SqlCommand("Select COUNT(Id) FROM Human", connection);
                if ((int)command1.ExecuteScalar() == 0)
                {
                    AddOneRecord();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select TOP 1 * FROM Human", connection);
                    command.ExecuteReader();
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "GetOneRecord", result.timing.ToString());
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
                SqlCommand command = new SqlCommand("Select COUNT(Id) FROM Human", connection);
                if ((int)command.ExecuteScalar() < 90000)
                {
                    AddMillionRecords();
                    Data.Clear();
                }
                connection.Close();
                for (int i = 0; i < 1; i++)
                {
                    MyTimer.StartTimer();

                    connection.Open();
                    command = new SqlCommand("SELECT * FROM Human", connection);
                    command.ExecuteReader();
                    connection.Close();

                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "GetAllRecords", result.timing.ToString());
                }



            }
            return Data;
        }

        public DataTable FindFirstNameColumn(string value)
        {
            Data.Clear();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                SqlCommand command;
                SqlParameter parameter;
                MyTimer.StartTimer();
                connection.Open();
                command = new SqlCommand($"SELECT * FROM Human WHERE FirstName LIKE'%@FirstName%'", connection);
                parameter = new SqlParameter("@FirstName", value);
                command.Parameters.Add(parameter);
                command.ExecuteReader();
                connection.Close();
                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "FindFirstNameColumn", result.timing.ToString());


            }
            return Data;
        }

        public DataTable UpdateOneRecord()
        {
            Data.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command1 = new SqlCommand("Select COUNT(Id) FROM Human", connection);
                if ((int)command1.ExecuteScalar() == 0)
                {
                    AddOneRecord();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE Human SET FirstName = 'FirstName' WHERE Id = (SELECT TOP 1 Id FROM Human)", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "UpdateOneRecord", result.timing.ToString());
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
                SqlCommand command1 = new SqlCommand("Select COUNT(Id) FROM Human", connection);
                if ((int)command1.ExecuteScalar() < 90000)
                {
                    AddMillionRecords();
                    Data.Clear();
                }
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE Human SET LastName = 'LastName'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "UpdateAllRecords", result.timing.ToString());
                }
            }

            return Data;
        }

        public DataTable DeleteOneRecord()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command1 = new SqlCommand("Select COUNT(Id) FROM Human", connection);
                if ((int)command1.ExecuteScalar() == 0)
                {
                    AddOneRecord();
                }
                Data.Clear();
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    SqlCommand command = new SqlCommand("Delete TOP(1) FROM Human", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "DeleteOneRecord", result.timing.ToString());
                }
            }

            return Data;
        }

        public DataTable DeleteAllRecord()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command1 = new SqlCommand("Select COUNT(Id) FROM Human", connection);
                if ((int)command1.ExecuteScalar() < 90000)
                {
                    AddMillionRecords();
                }
                Data.Clear();
                connection.Close();
                for (int time = 0; time < 1; time++)
                {
                    MyTimer.StartTimer();
                    connection.Open();
                    SqlCommand command = new SqlCommand("Delete FROM Human", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MyTimer result = MyTimer.StopTimer();
                    AddRow(this.Data, result.Endtime.ToString(), "Ado.NET", "DeleteAllRecord", result.timing.ToString());
                }
            }

            return Data;
        }
    }
}
