using System;

namespace Microsoft.Extensions.Logging.Splunk
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class SplunkLoggerProvider : ILoggerProvider
    {
        internal const string OriginalFormatPropertyName = "{OriginalFormat}";
        internal const string ScopePropertyName = "Scope";

        private readonly SplunkConfiguration configuration;
        private readonly Func<string, LogLevel, Exception, bool> filter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="configuration"></param>
        public SplunkLoggerProvider(Func<string, LogLevel, Exception, bool> filter, SplunkConfiguration configuration)
        {
            this.filter = filter;
            this.configuration = configuration;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new SplunkLogger(categoryName, filter, configuration);
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose of the object
        /// </summary>
        public void Dispose() {}
    }
}
