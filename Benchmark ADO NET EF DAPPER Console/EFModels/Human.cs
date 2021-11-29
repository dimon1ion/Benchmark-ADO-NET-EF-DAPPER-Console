using System;
using System.Collections.Generic;

#nullable disable

namespace Benchmark_ADO_NET_EF_DAPPER_Console.EFModels
{
    public partial class Human
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
    }
}
