using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class BoxingBenchmark
{
    [Params(1, 100, 10_000)]
    public int N { get; set; }

    public int Value { get; set; }

    private void AssignValueDirect(int value) => Value = value;
    private void AssignValueBoxed(object value) => Value = (int)value;
    private void AssignValueRefInterface<T>(in T value) where T : IComparable<int>, IEquatable<T> => Value = (int)(IComparable<int>)value;

    [Benchmark(Baseline = true)]
    public void DirectAssignment()
    {
        for (int i = 0; i < N; i++)
        {
            AssignValueDirect(i);
        }
    }

    [Benchmark]
    public void BoxedAssignment()
    {
        for (int i = 0; i < N; i++)
        {
            AssignValueBoxed(i);
        }
    }

    [Benchmark]
    public void InterfaceAssignment()
    {
        for (int i = 0; i < N; i++)
        {
            AssignValueRefInterface(in i);
        }
    }
}
