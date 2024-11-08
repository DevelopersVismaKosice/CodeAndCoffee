using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class ListIterarionBenchmark
{
    private List<int> _list;

    [Params(100, 10_000, 1_000_000)]
    public int N { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _list = new List<int>(Enumerable.Range(1, N));
    }

    [Benchmark(Baseline = true)]
    public void ForStatement()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _ = _list[i];
        }
    }

    [Benchmark]
    public void WhileStatement()
    {
        var i = 0;

        while (i < _list.Count)
        {
            _ = _list[i];
            i++;
        }
    }

    [Benchmark]
    public void ForEachStatement()
    {
        foreach (int item in _list)
        {
            _ = item;
        }
    }

    [Benchmark]
    public void ForEachMethod()
    {
        _list.ForEach(item => { _ = item; });
    }

    [Benchmark]
    public void ParallelForEach()
    {
        Parallel.ForEach(_list, item => { _ = item; });
    }

    [Benchmark]
    public void ForListSpan()
    {
        Span<int> listSpan = CollectionsMarshal.AsSpan(_list);

        for (int i = 0; i < listSpan.Length; i++)
        {
            _ = listSpan[i];
        }
    }

    [Benchmark]
    public void ForListRef()
    {
        Span<int> listSpan = CollectionsMarshal.AsSpan(_list);
        ref int firstItem = ref MemoryMarshal.GetReference(listSpan);

        for (int i = 0; i < listSpan.Length; i++)
        {
            _ = Unsafe.Add(ref firstItem, i);
        }
    }

    [Benchmark]
    public void WhileListRef()
    {
        Span<int> listSpan = CollectionsMarshal.AsSpan(_list);
        ref int firstItem = ref MemoryMarshal.GetReference(listSpan);
        ref int lastItem = ref Unsafe.Add(ref firstItem, listSpan.Length);

        while (Unsafe.IsAddressLessThan(ref firstItem, ref lastItem))
        {
            _ = firstItem;
            firstItem = ref Unsafe.Add(ref firstItem, 1);
        }
    }
}
