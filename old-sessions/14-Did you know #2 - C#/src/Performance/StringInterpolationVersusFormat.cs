using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
public class StringInterpolationVersusFormat
{
    private int[] array;

    [GlobalSetup]
    public void Setup()
    {
        array = Enumerable.Range(1, 10000).ToArray();
    }

    [Benchmark]
    public void StringInterpolated()
    {
        for(int i = 0; i < array.Length; i++)
        {
            System.Console.WriteLine($"StringInterpolated {array[i]}");
        }
    }

    [Benchmark]
    public void StringFormat()
    {
        for(int i = 0; i < array.Length; i++)
        {
            System.Console.WriteLine("StringFormat {0}", array[i]);
        }
    }

    [Benchmark]
    public void StringFormatOutside()
    {
        for(int i = 0; i < array.Length; i++)
        {
            string formatted = string.Format("StringFormatOutside {0}", array[i]);
            System.Console.WriteLine(formatted);
        }
    }

}
