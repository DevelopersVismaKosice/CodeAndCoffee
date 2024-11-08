using System.Reflection;
using BenchmarkDotNet.Running;
using PerformanceBenchmarks;

BenchmarkSwitcher switcher = new(Assembly.GetCallingAssembly());

switcher.Run(args, new Config());
