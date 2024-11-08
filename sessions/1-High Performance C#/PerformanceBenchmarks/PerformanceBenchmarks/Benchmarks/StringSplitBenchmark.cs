using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class StringSplitBenchmark
{
    private string _stringToSplit = string.Empty;
    private const char _splitChar = '.';

    [Params(3, 10, 100)]
    public int N { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < N; i++)
        {
            sb.Append("str").Append(i).Append(_splitChar);
        }

        sb.Length--;
        _stringToSplit = sb.ToString();
    }

    [Benchmark(Baseline = true)]
    public string[] StringSplit()
    {
        return _stringToSplit.Split(_splitChar, N);
    }

    [Benchmark]
    public string[] Substring()
    {
        int offset = 0;
        string[] splitItems = new string[N];

        for (int i = 0; i < N - 1; i++)
        {
            var splitIdx = _stringToSplit.IndexOf(_splitChar, offset);
            splitItems[i] = _stringToSplit.Substring(offset, splitIdx - offset);
            offset = splitIdx + 1;
        }

        splitItems[N - 1] = _stringToSplit.Substring(offset);

        return splitItems;
    }

    [Benchmark]
    public string[] SpanSlice()
    {
        int offset = 0;
        string[] splitItems = new string[N];
        var sourceSpan = _stringToSplit.AsSpan();

        for (int i = 0; i < N - 1; i++)
        {
            var splitIdx = sourceSpan.Slice(offset).IndexOf(_splitChar);
            splitItems[i] = sourceSpan.Slice(offset, splitIdx).ToString();
            offset += splitItems[i].Length + 1;
        }

        splitItems[N - 1] = sourceSpan.Slice(offset).ToString();

        return splitItems;
    }
}
