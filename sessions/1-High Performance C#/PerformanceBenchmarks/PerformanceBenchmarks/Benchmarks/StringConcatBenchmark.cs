using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class StringConcatBenchmark
{
    private string[] _strArr = [];
    private IEnumerable<string> _strEnumerable = [];

    [Params(3, 10, 100)]
    public int N { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        var stringList = new List<string>();

        for (int i = 0; i < N; i++)
        {
            stringList.Add($"string{i}");
        }

        _strEnumerable = stringList.AsEnumerable();
        _strArr = stringList.ToArray();
    }

    [Benchmark(Baseline = true)]
    public string StringConcatArray()
    {
        return string.Concat(_strArr);
    }

    [Benchmark]
    public string StringConcatEnumerable()
    {
        return string.Concat(_strEnumerable);
    }

    [Benchmark]
    public string StringInterpolation()
    {
        var result = string.Empty;

        foreach (string str in _strEnumerable)
        {
            result = $"{result}{str}";
        }

        return result;
    }

    [Benchmark]
    public string Addition()
    {
        var result = string.Empty;

        foreach (string str in _strEnumerable)
        {
            result += str;
        }

        return result;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var sb = new StringBuilder();

        foreach (string str in _strEnumerable)
        {
            sb.Append(str);
        }

        return sb.ToString();
    }

    [Benchmark]
    public string StringJoin()
    {
        return string.Join(null, _strEnumerable);
    }

    [Benchmark]
    public string StringCreateFromEnumerable()
    {
        int totalSize = _strEnumerable.Sum(s => s.Length);

        return string.Create(totalSize, _strEnumerable, (chars, state) =>
        {
            var offset = 0;

            foreach (var str in state)
            {
                str.AsSpan().CopyTo(chars.Slice(offset));
                offset += str.Length;
            }
        });
    }

    [Benchmark]
    public string StringCreateFromArray()
    {
        int totalSize = _strArr.Sum(s => s.Length);

        return string.Create(totalSize, _strArr, (chars, state) =>
        {
            var offset = 0;

            foreach (var str in state)
            {
                str.AsSpan().CopyTo(chars.Slice(offset));
                offset += str.Length;
            }
        });
    }
}
