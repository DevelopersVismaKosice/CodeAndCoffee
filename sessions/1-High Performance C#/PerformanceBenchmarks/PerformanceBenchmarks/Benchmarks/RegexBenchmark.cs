using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public partial class RegexBenchmark
{
    private const string _pattern = /* lang=regex */ @"\p{Sc}+\s*\d+";
    private const string _testString = "$ 100.2";

    private static readonly Regex StaticRegexInterpreted = new(_pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50));

    private static readonly Regex StaticRegexCompiled = new(_pattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(50));

    [Params(1, 100)]
    public int N { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Regex.CacheSize += 100;
    }

    [Benchmark(Baseline = true)]
    public void NewInstanceInterpreted()
    {
        for (int i = 0; i < N; i++)
        {
            Regex currencyRegex = new(_pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50));
            _ = currencyRegex.IsMatch(_testString);
        }
    }

    [Benchmark]
    public void NewInstanceCompiled()
    {
        for (int i = 0; i < N; i++)
        {
            Regex currencyRegex = new(_pattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(50));
            _ = currencyRegex.IsMatch(_testString);
        }
    }

    [Benchmark]
    public void StaticVariableInterpreted()
    {
        for (int i = 0; i < N; i++)
        {
            _ = StaticRegexInterpreted.IsMatch(_testString);
        }
    }

    [Benchmark]
    public void StaticVariableCompiled()
    {
        for (int i = 0; i < N; i++)
        {
            _ = StaticRegexCompiled.IsMatch(_testString);
        }
    }

    [Benchmark]
    public void StaticMethodInterpreted()
    {
        for (int i = 0; i < N; i++)
        {
            _ = Regex.IsMatch(_testString, _pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50));
        }
    }

    [Benchmark]
    public void StaticMethodCompiled()
    {
        for (int i = 0; i < N; i++)
        {
            _ = Regex.IsMatch(_testString, _pattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(50));
        }
    }

#if NET7_0_OR_GREATER
    [Benchmark]
    public void SourceGenerated()
    {
        for (int i = 0; i < N; i++)
        {
            _ = TestRegex().IsMatch(_testString);
        }
    }

    [GeneratedRegex(_pattern)]
    private static partial Regex TestRegex();
#endif
}
