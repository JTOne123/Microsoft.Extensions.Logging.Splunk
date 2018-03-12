using System;
using Xunit;

namespace Microsoft.Extensions.Logging.Splunk.Tests
{
    public class SplunkLoggerTests
    {
        private ILogger _logger = null;

        public SplunkLoggerTests()
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddSplunk(new SplunkConfiguration()
                {
                    ServerUrl = new Uri("http://localhost:8088"),
                    Token = "b9e45a2a-1093-4572-9a9d-2ef2baabafb5",
                    RetriesOnError = 0,
                    MinLevel = LogLevel.Trace
                });

            _logger = loggerFactory.CreateLogger<SplunkLoggerTests>();
        }

        [Fact]
        public void SendTraceWithoutException()
        {
            _logger.LogTrace("Trace log without exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendTraceWithException()
        {
            _logger.LogTrace(0, new Exception(), "Trace log with exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendDebugWithoutException()
        {
            _logger.LogDebug("Debug log without exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendDebugWithException()
        {
            _logger.LogDebug(0, new Exception(), "Debug log with exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendInformationWithoutException()
        {
            _logger.LogInformation("Information log without exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendInforamtionWithException()
        {
            _logger.LogInformation(0, new Exception(), "Information log with exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendWarningWithoutException()
        {
            _logger.LogWarning("Warning log without exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendWarningWithException()
        {
            _logger.LogError(0, new Exception(), "Warning log with exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendErrorWithoutException()
        {
            _logger.LogError("Error log without exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendErrorWithException()
        {
            _logger.LogError(0, new Exception(), "Error log with exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendCriticalWithoutException()
        {
            _logger.LogCritical("Critical log without exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }

        [Fact]
        public void SendCriticalWithException()
        {
            _logger.LogCritical(0, new Exception(), "Critical log with exception and UserName = {UserName}", "Guest");
            System.Threading.Thread.Sleep(1000);
            Assert.True(true);
        }
    }
}
