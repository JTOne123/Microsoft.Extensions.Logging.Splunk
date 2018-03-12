using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Splunk;
using System;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup a logger instance
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddSplunk(new SplunkConfiguration
                {
                    ServerUrl = new Uri("http://localhost:8088"),
                    Token = "b9e45a2a-1093-4572-9a9d-2ef2baabafb5",
                    RetriesOnError = 0,
                    MinLevel = LogLevel.Trace
                });
            ILogger logger = loggerFactory.CreateLogger<Program>();

            Console.WriteLine("Writting log messages...");
            // Write a few messages
            logger.LogTrace("This is a {errorTypeParam} log message", "trace");
            logger.LogDebug("This is a {errorTypeParam} log message", "debug");
            logger.LogInformation("This is an {errorTypeParam} log message", "information");
            logger.LogWarning("This is a {errorTypeParam} log message", "warning");
            logger.LogError("This is an {errorTypeParam} log message", "error");
            logger.LogCritical("This is a {errorTypeParam} log message", "critical");

            // Process an exception
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Our pretend exception detected!");
            }

#if DEBUG
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
#endif
        }
    }
}