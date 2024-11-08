using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class DynamicBenchmark
{
    private readonly MyClass _obj = new();
    private readonly dynamic _dynamicObj = (dynamic)(new MyClass());

    [Params(1, 100, 10_000)]
    public int N { get; set; }

    [Benchmark(Baseline = true)]
    public int DirectAccess()
    {
        int result = 0;

        for (int i = 0; i < N; i++)
        {
            result = _obj.Prop;
        }

        return result;
    }

    [Benchmark]
    public int DynamicAccess()
    {
        int result = 0;

        for (int i = 0; i < N; i++)
        {
            result = _dynamicObj.Prop;
        }

        return result;
    }

    private sealed class MyClass
    {
        public int Prop { get; set; } = 69;
    }
}
