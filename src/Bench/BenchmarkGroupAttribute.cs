using System;

namespace Bench
{
    public class BenchmarkGroupAttribute : Attribute
    {
        public BenchmarkGroupAttribute()
        {
        }

        public string Name { get; set; }
    }
}
