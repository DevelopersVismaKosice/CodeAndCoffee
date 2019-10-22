using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ForVersusForeach>();
            //var summary = BenchmarkRunner.Run<TextFileReading>();
        }
    }
}

