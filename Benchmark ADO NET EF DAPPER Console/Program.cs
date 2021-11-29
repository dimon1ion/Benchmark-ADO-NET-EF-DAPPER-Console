using Benchmark_ADO_NET_EF_DAPPER_Console.Executer;
using Benchmark_ADO_NET_EF_DAPPER_Console.Executer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Benchmark_ADO_NET_EF_DAPPER_Console
{
    class Program
    {
        static void WriteToConsole(DataTable data, bool column)
        {
            if (!column)
            {
                foreach (DataColumn column1 in data.Columns)
                {
                    Console.Write(column1.ColumnName + " ");
                }
                Console.WriteLine();
            }
            foreach (DataRow row in data.Rows)
            {
                Console.WriteLine(row["Resolved"] + " " + row["Resolver"] + " " + row["Name"] + " " + row["Timing"]);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Benchmark; Integrated Security=True;";
            int writeTask, writeResolver;
            List<IDbExecuter> executers;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter Task:" +
                    "\n1. AddMillionRecords" +
                    "\n2. AddOneRecord" +
                    "\n3. GetOneRecord" +
                    "\n4. GetAllRecords" +
                    "\n5. FindFirstNameColumn" +
                    "\n6. UpdateOneRecord" +
                    "\n7. UpdateAllRecords" +
                    "\n8. DeleteOneRecord" +
                    "\n9. DeleteAllRecord" +
                    "\n10. Break");
                writeTask = Int32.Parse(Console.ReadLine());
                if (0 < writeTask && writeTask < 10)
                {
                    executers = new List<IDbExecuter>();
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter Technology" +
                            "\n1. Ado.NET" +
                            "\n2. Entity Framework" +
                            "\n3. Dapper" +
                            "\n4. ALL");
                        writeResolver = Int32.Parse(Console.ReadLine());
                        switch (writeResolver)
                        {
                            case 1:
                                executers.Add(new AdoNetExecuter(connectionString));
                                break;
                            case 2:
                                executers.Add(new EFExecuter(connectionString));
                                break;
                            case 3:
                                executers.Add(new DapperExecuter(connectionString));
                                break;
                            case 4:
                                executers.Add(new AdoNetExecuter(connectionString));
                                executers.Add(new EFExecuter(connectionString));
                                executers.Add(new DapperExecuter(connectionString));
                                break;
                            default:
                                continue;
                        }
                        break;
                    }
                    bool column = false;
                    switch (writeTask)
                    {
                        case 1:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.AddMillionRecords(), column);
                                column = true;
                            }
                            break;
                        case 2:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.AddOneRecord(), column);
                                column = true;
                            }
                            break;
                        case 3:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.GetOneRecord(), column);
                                column = true;
                            }
                            break;
                        case 4:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.GetAllRecords(), column);
                                column = true;
                            }
                            break;
                        case 5:
                            Console.Write("Enter Firstname => ");
                            string firstName = Console.ReadLine();
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.FindFirstNameColumn(firstName), column);
                                column = true;
                            }
                            break;
                        case 6:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.UpdateOneRecord(), column);
                                column = true;
                            }
                            break;
                        case 7:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.UpdateAllRecords(), column);
                                column = true;
                            }
                            break;
                        case 8:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.DeleteOneRecord(), column);
                                column = true;
                            }
                            break;
                        case 9:
                            foreach (IDbExecuter item in executers)
                            {
                                WriteToConsole(item.DeleteAllRecord(), column);
                                column = true;
                            }
                            break;
                    }
                    Console.Write("Tap on some key..");
                    Console.ReadKey();
                    continue;
                }
                else if (writeTask != 10) { continue; }

                break;
            }
            //IDbExecuter executer = new DapperExecuter(connectionString);
            //DataTable result = executer.DeleteAllRecord();
            //foreach (DataColumn column in result.Columns)
            //{
            //    Console.Write(column.ColumnName + " ");
            //}
            //Console.WriteLine();
            //foreach (DataRow row in result.Rows)
            //{
            //    Console.WriteLine(row["Resolved"] + " " + row["Resolver"] + " " + row["Name"] + " " + row["Timing"]);
            //}
        }
    }
}
