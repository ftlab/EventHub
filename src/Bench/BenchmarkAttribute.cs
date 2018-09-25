using System;

namespace Bench
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BenchmarkAttribute : Attribute
    {
        public BenchmarkAttribute()
        {
        }

        public string Name { get; set; }

        public int TestCount { get; set; } = 1000;
    }
}
