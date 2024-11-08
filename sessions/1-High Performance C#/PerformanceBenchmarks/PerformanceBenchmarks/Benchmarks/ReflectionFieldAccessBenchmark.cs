using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class ReflectionFieldAccessBenchmark
{
    private readonly MyClass _obj = new();
    private static readonly FieldInfo _fieldInfo = typeof(MyClass)
            .GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    [Params(1, 100, 10_000)]
    public int N { get; set; }

    [Benchmark(Baseline = true)]
    public void DirectAccess()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _obj._publicField;
        }
    }

    [Benchmark]
    public void DynamicAccess()
    {
        for (int i = 0; i < N; i++)
        {
            _ = typeof(MyClass)
                .GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(_obj);
        }
    }

    [Benchmark]
    public void CachedDynamicAccess()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _fieldInfo.GetValue(_obj);
        }
    }

#if NET8_0_OR_GREATER
    [Benchmark]
    public void UnsafeAccessor()
    {
        for (int i = 0; i < N; i++)
        {
            _ = GetInstanceField(_obj);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_privateField")]
    extern static ref int GetInstanceField(MyClass @this);
#endif

    private sealed class MyClass
    {
        public readonly int _publicField = 69;
        private readonly int _privateField = 69;
    }
}
