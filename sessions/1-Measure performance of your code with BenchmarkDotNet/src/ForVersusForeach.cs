using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Collections.Generic;

namespace src
{
    [RPlotExporter, RankColumn]
    [MemoryDiagnoser]
    public class ForVersusForeach
    {
        [Params(1000, 10000, 100000)]
        public int N;

        public List<int> IterationCollection;

        [GlobalSetup]
        public void Setup()
        {
            IterationCollection = new List<int>(Enumerable.Range(1, N));
        } 

        [Benchmark]
        public void IterateOverFor()
        {
            for(int index = 0; index < N; index++)
            {
                var x = IterationCollection[index] ^ 5;
            }
        }

        [Benchmark]
        public void IterateOverForeach()
        {
            foreach(var item in IterationCollection)
            {
                var x = item ^ 5;
            }
        }
    }
}