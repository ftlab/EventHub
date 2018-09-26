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
            var groups = assembly.GetTypes()
                .Select(t => new
                {
                    Type = t,
                    Group = t.GetCustomAttribute<BenchmarkGroupAttribute>()
                })
                .OrderBy(g => g.Group?.Name ?? g.Type.Name).ToArray();

            foreach (var group in groups)
            {
                var groupName = group.Group?.Name ?? group.Type.Name;

                var methods = group.Type.GetMethods().Select(m => new
                {
                    Method = m,
                    Benchmark = m.GetCustomAttribute<BenchmarkAttribute>()
                })
                .Where(b => b.Benchmark != null)
                .OrderBy(b => b.Benchmark?.Name ?? b.Method.Name).ToArray();

                if (methods.Any())
                {
                    ResultWriter.OnGroup(groupName);

                    foreach (var method in methods)
                    {
                        var benchmarkName = string.IsNullOrEmpty(method.Benchmark.Name) ? method.Method.Name : method.Benchmark.Name;
                        var info = new BenchmarkInfo()
                        {
                            Group = groupName,
                            Name = benchmarkName,
                            TestCount = method.Benchmark.TestCount,
                        };

                        var benchmark = new Benchmark(method.Method, info);
                        var result = benchmark.Run();

                        ResultWriter.OnResult(info, result);
                    }
                }
            }
        }
    }
}
