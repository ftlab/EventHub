using Bench;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestEventHub
{
    class Program
    {
        static void Main(string[] args)
        {
            Benchmarks.Run();

            Console.WriteLine($"Завершено");
            Console.ReadKey();
        }

        public static Random NGen = new Random();
        public static double[] Array = Enumerable.Repeat(0, 1000).Select(i => NGen.NextDouble()).ToArray();

        [Benchmark]
        public static void TestAverage()
        {
            var v = Array.Average();
        }
    }
}
