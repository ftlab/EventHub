using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Bench
{
    public class Benchmark
    {
        public Benchmark(MethodInfo method, int testCount)
        {
            if (testCount < 1) throw new ArgumentException("testCount < 1");
            Method = method ?? throw new ArgumentNullException(nameof(method));
            TestCount = testCount;
        }

        public MethodInfo Method { get; }

        public int TestCount { get; }

        public string Group { get; }

        public string Name { get; }

        public BenchmarkResult Run()
        {
            if (Method.IsStatic == false)
                return new BenchmarkResult() { Comment = $"Метод {Method.Name} должен быть статичным" };

            //FIXME
            if (Method.IsGenericMethod)
                return new BenchmarkResult() { Comment = $"Обобщенный метод {Method.Name} не поддерживаются" };

            if (Method.GetParameters().Any())
                return new BenchmarkResult() { Comment = $"Метод {Method.Name} должен быть без параметров" };

            var sw = new Stopwatch();
            var b = new BenchmarkResult();
            var cnt = TestCount;
            while (cnt > 0)
            {
                sw.Reset();
                sw.Start();
                try
                {
                    Method.Invoke(null, null);
                    sw.Stop();

                    b = b.Next(sw.Elapsed);
                }
                catch (Exception e)
                {
                    b.Comment = $"Ошибка: {e.Message}";
                }
                finally
                {
                    sw.Stop();
                    cnt--;
                }
            }

            return b;
        }
    }
}
