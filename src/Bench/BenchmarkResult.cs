using System;
using System.Collections.Generic;
using System.Linq;

namespace Bench
{
    public class BenchmarkResult
    {
        private List<long> _ticksSet = new List<long>();

        public string LastError { get; set; }

        public int ErrorCount { get; set; }

        public int Count => _ticksSet.Count;

        public long TotalTicks => _ticksSet.Sum();

        public long MinTicks => _ticksSet.Min();

        public long MaxTicks => _ticksSet.Max();

        public long AvgTicks => (long)Math.Ceiling(_ticksSet.Average());

        public TimeSpan Total => TimeSpan.FromTicks(TotalTicks);

        public TimeSpan Min => TimeSpan.FromTicks(MinTicks);

        public TimeSpan Max => TimeSpan.FromTicks(MaxTicks);

        public TimeSpan Avg => TimeSpan.FromTicks(AvgTicks);

        public override string ToString()
        {
            return $"Count: {Count}, Avg: {Avg}, Total: {Total}, Min: {Min}, Max: {Max}, ErrorCount: {ErrorCount}, LastError: {LastError}";
        }

        public void Next(long ticks)
        {
            _ticksSet.Add(ticks);
        }

        public void SetError(string error)
        {
            LastError = error;
            ErrorCount = ErrorCount + 1;
        }
    }
}
