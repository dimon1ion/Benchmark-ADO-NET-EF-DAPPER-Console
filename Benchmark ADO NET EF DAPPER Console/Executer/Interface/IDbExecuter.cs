using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Benchmark_ADO_NET_EF_DAPPER_Console.Executer.Interface
{
    public interface IDbExecuter
    {
        public string ConnectionString { get; set; }
        DataTable AddMillionRecords();
        DataTable AddOneRecord();
        DataTable GetOneRecord();
        DataTable GetAllRecords();
        DataTable FindFirstNameColumn(string value);
        DataTable UpdateOneRecord();
        DataTable UpdateAllRecords();
        DataTable DeleteOneRecord();
        DataTable DeleteAllRecord();
    }
}
