using System;

namespace Bench
{
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

        public Benchmark Next(TimeSpan ts)
        {
            var b = new Benchmark();
            b.Count = Count + 1;
            b.Min = ts < Min ? ts : Min;
            b.Max = ts > Max ? ts : Max;
            b.Avg = TimeSpan.FromMilliseconds(
                Average(Avg.TotalMilliseconds, ts.TotalMilliseconds, b.Count)
                );
            return b;
        }

        private static double Average(double prev, double next, double n)
        {
            return ((n - 1) / n) * prev + next / n;
        }
    }
}
