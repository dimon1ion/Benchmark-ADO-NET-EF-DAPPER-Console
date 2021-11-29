using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Benchmark_ADO_NET_EF_DAPPER_Console
{
    public class MyTimer
    {
        public DateTime Endtime { get; set; }
        public double timing { get; set; }

        private static Stopwatch stopwatch = new Stopwatch();
        
        public static void StartTimer()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
            stopwatch.Start();
        }
        public static MyTimer StopTimer()
        {
            if (stopwatch.IsRunning)
            {
                DateTime stop = DateTime.Now;
                stopwatch.Stop();
                //Console.WriteLine(stopwatch.Elapsed.TotalSeconds);
                MyTimer result = new MyTimer() { Endtime = stop, timing = stopwatch.Elapsed.TotalSeconds };
                stopwatch.Reset();
                return result;
            }
            stopwatch.Reset();
            return null;
        }
    }
}
