using Benchmark_ADO_NET_EF_DAPPER_Console.Executer.Interface;
using Benchmark_ADO_NET_EF_DAPPER_Console.EFModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Benchmark_ADO_NET_EF_DAPPER_Console.Executer
{
    class EFExecuter : IDbExecuter
    {
        public string ConnectionString { get; set; }
        public DataTable Data { get; set; }

        private BenchmarkContext context;

        public EFExecuter(string connectionString)
        {
            ConnectionString = connectionString;
            context = new BenchmarkContext(connectionString);
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
            Random rnd = new Random();
            context.Humans.RemoveRange(context.Humans);
            context.SaveChanges();

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();
                for (int i = 1; i <= 100000; i++)
                {
                    context.Humans.Add(new Human() { FirstName = $"FirstName{i}", LastName = $"LastName{i}", Age = rnd.Next(5, 26), Gender = rnd.Next(0, 2) == 1 });
                }
                context.SaveChanges();
                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "AddMillionRecords", result.timing.ToString());
            }

            return Data;
        }

        public DataTable AddOneRecord()
        {
            Data.Clear();
            Random rnd = new Random();

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();
                context.Humans.Add(new Human() { FirstName = $"FirstName0{rnd.Next(0, 100)}", LastName = $"LastName0{rnd.Next(0, 100)}", Age = rnd.Next(5, 26), Gender = rnd.Next(0, 2) == 1 });
                context.SaveChanges();
                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "AddOneRecord", result.timing.ToString());
            }

            return Data;
        }

        public DataTable GetOneRecord()
        {
            Data.Clear();
            if (context.Humans.Count() == 0)
            {
                AddOneRecord();
                Data.Clear();
            }
            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();
                context.Humans.FirstOrDefault();
                context.SaveChanges();
                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "GetOneRecord", result.timing.ToString());
            }
            return Data;
        }

        public DataTable GetAllRecords()
        {
            Data.Clear();

            if (context.Humans.Count() < 90000)
            {
                AddMillionRecords();
                Data.Clear();
            }

            MyTimer.StartTimer();
            List<Human> humen = context.Humans.ToList();
            MyTimer result = MyTimer.StopTimer();
            AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "GetAllRecords", result.timing.ToString());

            return Data;
        }

        public DataTable FindFirstNameColumn(string value)
        {
            Data.Clear();

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();

                context.Humans.Where(p => p.FirstName.Contains(value)).ToList();

                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "FindFirstNameColumn", result.timing.ToString());
            }

            return Data;
        }

        public DataTable UpdateOneRecord()
        {
            Data.Clear();

            if (context.Humans.Count() == 0)
            {
                AddOneRecord();
                Data.Clear();
            }

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();

                context.Humans.First().FirstName = "FirstName";
                context.SaveChanges();

                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "UpdateOneRecord", result.timing.ToString());
            }

            return Data;
        }

        public DataTable UpdateAllRecords()
        {
            Data.Clear();

            if (context.Humans.Count() < 90000)
            {
                AddMillionRecords();
                Data.Clear();
            }

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();

                DbSet<Human> humen = context.Humans;
                foreach (var human in humen)
                {
                    human.LastName = "LastName";
                }
                context.SaveChanges();

                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "UpdateAllRecords", result.timing.ToString());
            }

            return Data;
        }

        public DataTable DeleteOneRecord()
        {
            if (context.Humans.Count() == 0)
            {
                AddOneRecord();
            }
            Data.Clear();

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();

                context.Humans.Remove(context.Humans.First());
                context.SaveChanges();

                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "DeleteOneRecord", result.timing.ToString());
            }

            return Data;
        }

        public DataTable DeleteAllRecord()
        {
            if (context.Humans.Count() < 90000)
            {
                AddMillionRecords();
            }
            Data.Clear();

            for (int time = 0; time < 1; time++)
            {
                MyTimer.StartTimer();

                context.Humans.RemoveRange(context.Humans);
                context.SaveChanges();

                MyTimer result = MyTimer.StopTimer();
                AddRow(this.Data, result.Endtime.ToString(), "EntityFramework", "DeleteAllRecord", result.timing.ToString());
            }

            return Data;
        }
    }
}
