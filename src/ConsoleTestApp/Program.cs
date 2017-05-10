using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Splunk;
using System;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddSplunk(new SplunkConfiguration() { ServerUrl = new Uri("https://localhost:8088"), Token = "ED9F5A37-BE9A-4782-B5F7-B6E31AC369CA", RetriesOnError = 0, IgnoreSslErrors = true, MinLevel = LogLevel.Trace });

            ILogger logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("This is a logging test message!");


            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}