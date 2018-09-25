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
            var br = Test(1000, () =>
            {
                AutoResetEvent e = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    var r = 9 + 9;
                    Console.WriteLine(1);
                    e.Set();
                });
                e.WaitOne();
            });
            Console.WriteLine(br);


            Console.WriteLine($"Завершено");
            Console.ReadKey();
        }

        public static Benchmark Test(int count, Action action)
        {
            var b = new Benchmark();
            b.Count = count;
            var sw = new Stopwatch();

            double n = 0;
            while (count > 0)
            {
                count--;
                n++;

                sw.Reset();
                sw.Start();

                action();

                sw.Stop();

                var elapsed = sw.Elapsed;

                b.Total += elapsed;
                b.Min = elapsed < b.Min ? elapsed : b.Min;
                b.Max = elapsed > b.Max ? elapsed : b.Max;
                b.Avg = TimeSpan.FromMilliseconds(
                    Stat.Avg(b.Avg.TotalMilliseconds, elapsed.TotalMilliseconds, n));
            }

            return b;
        }

        public class Benchmark
        {
            public int Count;

            public TimeSpan Total;

            public TimeSpan Avg;

            public TimeSpan Min = TimeSpan.MaxValue;

            public TimeSpan Max = TimeSpan.MinValue;

            public override string ToString()
            {
                return $"Count: {Count},Total: {Total}, avg: {Avg}, Min: {Min}, Max: {Max}";
            }
        }
    }
}
