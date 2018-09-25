using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bench
{
    public static class Benchmarks
    {
        public static void Run()
        {
            var assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes();

            var groups = types.Select(t => new
            {
                Type = t,
                Group = t.GetCustomAttribute<BenchmarkGroupAttribute>()
            }).ToArray();

            var methods = groups.SelectMany(g =>
                g.Type.GetMethods().Select(m => new
                {
                    Method = m,
                    g.Group,
                    Benchmark = m.GetCustomAttribute<BenchmarkAttribute>()
                }))
                .Where(m => m.Benchmark != null).ToArray();

            var benchmarks = methods
                .Select(m => new
                {
                    Benchmark = new Benchmark(m.Method, m.Benchmark.TestCount),
                    Group = m.Group?.Name,
                    Name = m.Benchmark.Name ?? m.Method.Name,
                }).ToArray();

            foreach (var b in benchmarks)
            {
                var r = b.Benchmark.Run();
            }
        }

    }
}
