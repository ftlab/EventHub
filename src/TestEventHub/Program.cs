using Bench;
using System;
using System.Linq;

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

        [BenchmarkGroup(Name = "Мои тесты")]
        public class MyTests
        {
            [Benchmark(Name = "Тест 1")]
            public static void Test2()
            {
            }

            [Benchmark(Name = "Тест 2")]
            public void Test3()
            {
            }
        }
    }
}
