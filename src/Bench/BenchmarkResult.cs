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

        public string LastError;

        public int ErrorCount;

        public override string ToString()
        {
            return $"Count: {Count}, Avg: {Total}, Total: {Avg}, Min: {Min}, Max: {Max}, ErrorCount: {ErrorCount}, LastError: {LastError}";
        }

        public BenchmarkResult Next(TimeSpan ts)
        {
            var b = new BenchmarkResult();
            b.Count = Count + 1;
            b.Total += ts;
            b.Min = ts < Min ? ts : Min;
            b.Max = ts > Max ? ts : Max;
            b.Avg = TimeSpan.FromMilliseconds(
                Average(Avg.TotalMilliseconds, ts.TotalMilliseconds, b.Count)
                );
            b.LastError = LastError;
            return b;
        }

        public BenchmarkResult SetError(string error)
        {
            var b = new BenchmarkResult();
            b.Count = Count;
            b.Total = Total;
            b.Min = Min;
            b.Max = Max;
            b.Avg = Avg;
            b.LastError = error;
            b.ErrorCount = ErrorCount + 1;
            return b;
        }

        private static double Average(double prev, double next, double n)
        {
            return ((n - 1) / n) * prev + next / n;
        }
    }
}
