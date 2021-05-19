using System;

namespace DidYouKnowCSharp2.Gotchas
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class LocalLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("Writing from LocalLogger Log method: ", message);
        }
    }

    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            Console.WriteLine("Writing from static extension method Log method: ", message);
        }
    }
}