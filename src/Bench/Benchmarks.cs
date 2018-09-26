using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bench
{
    public static class Benchmarks
    {
        public static BencmarkResultWriter ResultWriter = new BencmarkResultWriter();

        public static void Run()
        {
            var assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes();

            var groups = types.Select(t => new
            {
                Type = t,
                Group = t.GetCustomAttribute<BenchmarkGroupAttribute>()?.Name ?? t.Name,
                Bencmarks = t.GetMethods().Select(m => new
                {
                    Method = m,
                    BenchmarkAttribute = m.GetCustomAttribute<BenchmarkAttribute>()
                }).Where(b => b.BenchmarkAttribute != null)
                .OrderBy(b => b.BenchmarkAttribute.Name).ToArray(),
            })
            .Where(g => g.Bencmarks.Any()).OrderBy(g => g.Group).ToArray();

            foreach (var group in groups)
            {
                ResultWriter.OnGroup(group.Group);

                foreach (var binfo in group.Bencmarks)
                {
                    var benchmark = new Benchmark(binfo.Method, binfo.BenchmarkAttribute.TestCount);
                    var result = benchmark.Run();

                    ResultWriter.OnResult(benchmark, result);
                }
            }
        }
    }
}
