using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Bench
{
    public class Benchmark
    {
        public Benchmark(MethodInfo method, BenchmarkInfo info)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

        public MethodInfo Method { get; }

        public BenchmarkInfo Info { get; }

        public BenchmarkResult Run()
        {
            //FIXME
            if (Method.IsGenericMethod)
                return new BenchmarkResult() { LastError = $"Обобщенный метод {Method.Name} не поддерживается" };

            if (Method.GetParameters().Any())
                return new BenchmarkResult() { LastError = $"Метод {Method.Name} должен быть без параметров" };

            object context;
            if (Method.IsStatic)
                context = null;
            else
            {
                var constructor = Method.ReflectedType.GetConstructors()
                    .Where(c => c.GetParameters().Where(p => p.HasDefaultValue == false).Any() == false)
                    .FirstOrDefault();

                if (constructor == null)
                    return new BenchmarkResult() { LastError = $"Конструктор {Method.ReflectedType.Name} должен быть без параметров" };

                context = constructor.Invoke(null);
            }

            var sw = new Stopwatch();
            var b = new BenchmarkResult();
            var cnt = Info.TestCount;
            while (cnt > 0)
            {
                sw.Reset();
                sw.Start();
                try
                {
                    Method.Invoke(context, null);
                    sw.Stop();

                    b.Next(sw.ElapsedTicks);
                }
                catch (Exception e)
                {
                    b.SetError($"Ошибка: {e.Message}");
                    Benchmarks.ResultWriter.OnError(Info, e);
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
