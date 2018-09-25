using System;

namespace Bench
{
    public class BenchmarkResult
    {
        public int Count;

        public TimeSpan Total;

        public TimeSpan Avg;

        public TimeSpan Min = TimeSpan.MaxValue;

        public TimeSpan Max = TimeSpan.MinValue;

        public string Comment;

        public override string ToString()
        {
            return $"Count: {Count},Total: {Total}, avg: {Avg}, Min: {Min}, Max: {Max}, Comment: {Comment}";
        }

        public BenchmarkResult Next(TimeSpan ts)
        {
            var b = new BenchmarkResult();
            b.Count = Count + 1;
            b.Min = ts < Min ? ts : Min;
            b.Max = ts > Max ? ts : Max;
            b.Avg = TimeSpan.FromMilliseconds(
                Average(Avg.TotalMilliseconds, ts.TotalMilliseconds, b.Count)
                );
            b.Comment = Comment;
            return b;
        }

        private static double Average(double prev, double next, double n)
        {
            return ((n - 1) / n) * prev + next / n;
        }
    }
}
