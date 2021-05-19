
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
public class SpeedItUp
{
    private int[] array;

    [GlobalSetup]
    public void Setup()
    {
        array = Enumerable.Range(1, 1000000).ToArray();
    }

    [Benchmark]
    public int SumOdd()
    {
        int counter = 0;
        for(int i = 0; i < array.Length; i++)
        {
            var element = array[i];
            if(element % 2 != 0)
            {
                counter += element;
            }
        }
        return counter;    
    }

    [Benchmark]
    // % modulo is quite expensive and JIT compiler automatically uses Shift left and And trick on the modulo
    public int SumOdd_Bit()
    {
        int counter = 0;
        for(int i = 0; i < array.Length; i++)
        {
            var element = array[i];
            if((element & 1) == 1)
            {
                counter += element;
            }
        }
        return counter;    
    }

    [Benchmark]
    // Branch prediction is expensive, but removing branch might also be expensive
    public int SumOdd_BranchFree()
    {
        int counter = 0;
        for(int i = 0; i < array.Length; i++)
        {
            var element = array[i];
            var odd = element & 1;
            counter += (odd * element);
        }
        return counter;    
    }

    [Benchmark]
    // Branch prediction is expensive, but removing branch might also be expensive
    public int SumOdd_BranchFree_Parallel()
    {
        int counterA = 0;
        int counterB = 0;

        for(int i = 0; i < array.Length; i += 2) // !
        {
            var elementA = array[i];
            var elementB = array[i + 1];
            
            var oddA = elementA & 1;
            var oddB = elementB & 1;

            counterA += (oddA * elementA);
            counterB += (oddB * elementB);
        }
        return counterA + counterB;    
    }

    // next: using pointers ...
}