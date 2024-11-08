using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class StringCIComparisonBenchmark
{
    [Params(1, 10, 50)]
    public int N { get; set; }

    private string _leftString;
    private string _rightString;

    [IterationSetup]
    public void IterationSetup()
    {
        _leftString = new string('a', N);
        _rightString = new string('A', N);
    }

    [Benchmark(Baseline = true)]
    public void ToLowerEqualityOperator()
    {

        _ = _leftString.ToLower() == _rightString.ToLower();
    }

    [Benchmark]
    public void ToLowerEquals()
    {
        _ = string.Equals(_leftString.ToLower(), _rightString.ToLower());
    }

    [Benchmark]
    public void EqualsOrdinalIgnoreCase()
    {
        _ = string.Equals(_leftString, _rightString, StringComparison.OrdinalIgnoreCase);
    }
}
