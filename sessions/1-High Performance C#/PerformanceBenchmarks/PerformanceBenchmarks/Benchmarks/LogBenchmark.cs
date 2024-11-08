using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PerformanceBenchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
public partial class LoggerBenchmark
{
    private const string _template = "The time is {Now}";
    private readonly ILogger<LoggerBenchmark> _logger;
    private readonly DateTime _dateTime = DateTime.Now;
    private static readonly Action<ILogger, DateTime, Exception?> _loggerMessage = LoggerMessage.Define<DateTime>(
        LogLevel.Information,
        new EventId(1),
        _template);

    public LoggerBenchmark()
    {
        _logger = new LoggerFactory().CreateLogger<LoggerBenchmark>();
    }

    [Benchmark(Baseline = true)]
    public void Basic()
    {
        _logger.LogInformation("The time is {Now}", _dateTime);
    }

    [Benchmark]
    public void ConstantMessageTemplate()
    {
        _logger.LogInformation(_template, _dateTime);
    }

    [Benchmark]
    public void DynamicMessageTemplate()
    {
        _logger.LogInformation($"The time is {_dateTime}");
    }

    [Benchmark]
    public void LogLevelCheck()
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(_template, _dateTime);
        }
    }

    [Benchmark]
    public void LoggerMessageLog()
    {
        _loggerMessage(_logger, _dateTime, null);
    }

    [Benchmark]
    public void SourceGeneratedLog()
    {
        LogTime(_dateTime);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = _template)]
    private partial void LogTime(DateTime now);
}
