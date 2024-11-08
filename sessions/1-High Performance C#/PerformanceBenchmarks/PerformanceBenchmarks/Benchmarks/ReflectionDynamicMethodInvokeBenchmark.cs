using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class ReflectionDynamicMethodInvokeBenchmark
{
    private delegate int AddDelegate(int value);
    private readonly MyClass _obj = new();
    private static readonly MethodInfo _addMethod = typeof(MyClass).GetMethod(nameof(MyClass.Add))!;
    private AddDelegate _addDelegate;

    [Params(1, 100, 10_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _addDelegate = (AddDelegate)Delegate.CreateDelegate(typeof(AddDelegate), _obj, nameof(MyClass.Add));
    }

    [Benchmark(Baseline = true)]
    public void DirectInvoke()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _obj.Add(i);
        }
    }

    [Benchmark]
    public void MethodInfoInvoke()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _addMethod.Invoke(_obj, [i]);
        }
    }

    [Benchmark]
    public void DelegateInvoke()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _addDelegate.Invoke(i);
        }
    }

    [Benchmark]
    public void DelegateDynamicInvoke()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _addDelegate.DynamicInvoke(i);
        }
    }

    private sealed class MyClass
    {
        private int _value;

        public int Add(int arg)
        {
            _value += arg;

            return _value;
        }
    }
}
