using System;
using System.Collections.Generic;
using System.Text;

namespace Bench
{
    public class BencmarkResultWriter
    {
        public virtual void OnGroup(string name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(name);
            Console.ResetColor();
        }

        public virtual void OnResult(Benchmark benchmark, BenchmarkResult result)
        {
            Console.WriteLine($"\t{benchmark.Name}. {result}.");
        }
    }
}
