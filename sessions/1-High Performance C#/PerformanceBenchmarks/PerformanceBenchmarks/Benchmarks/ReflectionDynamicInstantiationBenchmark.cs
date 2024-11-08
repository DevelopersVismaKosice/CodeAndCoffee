using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public class ReflectionDynamicInstantiationBenchmark
{
    public delegate T ObjectActivator<out T>(params object[] args);
    private static readonly ConstructorInfo Ctor = typeof(DataClass).GetConstructor(Type.EmptyTypes)!;
    private ObjectActivator<DataClass> _activator = default!;
    private Func<object> _dynamicMethodActivator = default!;

    [Params(1, 100, 10_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _activator = GetActivator<DataClass>(Ctor);

        DynamicMethod createDataMethod = new(
            "DynamicMethodData",
            typeof(DataClass),
            null,
            typeof(ReflectionDynamicInstantiationBenchmark).Module,
            false);

        ILGenerator il = createDataMethod.GetILGenerator();
        il.Emit(OpCodes.Newobj, typeof(DataClass).GetConstructors()[0]);
        il.Emit(OpCodes.Ret);

        _dynamicMethodActivator = (Func<object>)createDataMethod.CreateDelegate(typeof(Func<object>));
    }

    [Benchmark(Baseline = true)]
    public void New()
    {
        for (int i = 0; i < N; i++)
        {
            _ = new DataClass();
        }
    }

    [Benchmark]
    public void ActivatorCreateInstance()
    {
        for (int i = 0; i < N; i++)
        {
            _ = Activator.CreateInstance(typeof(DataClass));
        }
    }

    [Benchmark]
    public void ConstructorInfo()
    {
        for (int i = 0; i < N; i++)
        {
            _ = Ctor.Invoke(null);
        }
    }

    [Benchmark]
    public void ObjectActivatorDelegate()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _activator();
        }
    }

    [Benchmark]
    public void ReflectionEmit()
    {
        for (int i = 0; i < N; i++)
        {
            _ = _dynamicMethodActivator();
        }
    }

    private static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor)
    {
        ParameterInfo[] paramsInfo = ctor.GetParameters();
        ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
        Expression[] argsExp = new Expression[paramsInfo.Length];

        for (int i = 0; i < paramsInfo.Length; i++)
        {
            Expression index = Expression.Constant(i);
            Type paramType = paramsInfo[i].ParameterType;
            Expression paramAccessorExp = Expression.ArrayIndex(param, index);
            Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

            argsExp[i] = paramCastExp;
        }

        NewExpression newExp = Expression.New(ctor, argsExp);
        LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);
        ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();

        return compiled;
    }
}

public class DataClass
{
    public string? Value { get; set; }
    public string? DisplayFormat { get; set; }

    public DataClass() { }

    public DataClass(string value, string displayFormat)
    {
        Value = value;
        DisplayFormat = displayFormat;
    }
}