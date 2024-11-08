using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class SealedBenchmark
{
    private readonly NonSealedType _nonSealed = new();
    private readonly SealedType _sealed = new();

    [Benchmark(Baseline = true)]
    public int NonSealed() => _nonSealed.Method();

    [Benchmark]
    public int Sealed() => _sealed.Method();
}

public class BaseType
{
    public virtual int Method() => 1 + 2;
}

public class NonSealedType : BaseType
{
    public override int Method() => 6 + 9;
}

public sealed class SealedType : BaseType
{
    public override int Method() => 6 + 9;
}
