using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[HideColumns("Job", "Error", "StdDev", "RatioSD")]
public class LinqBenchmark
{
    private readonly IEnumerable<int> _items = Enumerable.Range(0, 1000).ToList();

    [Benchmark]
    public bool Any() => _items.Any(x => x == 1000);

    [Benchmark]
    public bool All() => _items.All(x => x >= 0);

    [Benchmark]
    public int Count() => _items.Count(x => x == 0);

    [Benchmark]
    public int First() => _items.First(x => x == 500);

    [Benchmark]
    public int Single() => _items.Count(x => x == 0);
}
