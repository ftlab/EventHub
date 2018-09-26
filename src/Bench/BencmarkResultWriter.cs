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

        public virtual void OnResult(BenchmarkInfo benchmark, BenchmarkResult result)
        {
            Console.WriteLine($"{benchmark.Name}. {result}.");
        }

        public virtual void OnError(BenchmarkInfo benchmark, Exception e)
        {
        }
    }
}
