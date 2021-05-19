using BenchmarkDotNet.Running;
using DidYouKnowCSharp2.Gotchas;
using System;

namespace DidYouKnowCSharp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<SpeedItUp>();

            var summary = BenchmarkRunner.Run<StringInterpolationVersusFormat>();

            ILogger logger = new LocalLogger();
            logger.Log("hello there");            
        }
    }
}
