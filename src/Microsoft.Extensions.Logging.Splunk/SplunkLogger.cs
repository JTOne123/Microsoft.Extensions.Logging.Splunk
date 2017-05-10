using Splunk.Logging;
using System;
using System.Dynamic;
using System.Net;

namespace Microsoft.Extensions.Logging.Splunk
{
    /// <summary>
    /// 
    /// </summary>
    public class SplunkLogger : ILogger
    { 
        private readonly HttpEventCollectorSender hecSender;
        private readonly SplunkConfiguration configuration;
        private readonly string name;
        private Func<string, LogLevel, Exception, bool> filter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <param name="environmentName"></param>
        /// <param name="applicationName"></param>
        /// <param name="configuration"></param>
        public SplunkLogger(string name, Func<string, LogLevel, Exception, bool> filter, SplunkConfiguration configuration)
        {
            Filter = filter ?? ((category, logLevel, exception) => true);
            this.configuration = configuration;
            this.name = name;

            hecSender = new HttpEventCollectorSender(
                configuration.ServerUrl,                                                                // Splunk HEC URL
                configuration.Token,                                                                    // Splunk HEC token *GUID*
                new HttpEventCollectorEventInfo.Metadata(null, null, "_json", "machinename"), // Metadata
                HttpEventCollectorSender.SendMode.Sequential,                                           // Sequential sending to keep message in order
                0,                                                                                      // BatchInterval - Set to 0 to disable
                0,                                                                                      // BatchSizeBytes - Set to 0 to disable
                0,                                                                                      // BatchSizeCount - Set to 0 to disable
                new HttpEventCollectorResendMiddleware(configuration.RetriesOnError).Plugin             // Resend Middleware with retry
            );

            hecSender.OnError += exception => 
            {
                throw new Exception($"SplunkLogger failed to send log event to Splunk server '{configuration.ServerUrl.Authority}' using token '{configuration.Token}'. Exception: {exception}");
            };

            // If enabled will create callback to bypass ssl error checks for our server url
            if (configuration.IgnoreSslErrors)
            {
                // TODO: add ssl error checker for .net core
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Func<string, LogLevel, Exception, bool> Filter
        {
            get { return filter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                filter = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return IsEnabled(logLevel, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel, Exception ex)
        {
            return Filter(name, logLevel, ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel, exception))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            // Make sure we have a properly setup HttpEventCollectorSender
            if (hecSender == null)
            {
                throw new NullReferenceException("SplunkLogger HttpEventCollectorSender object was null");
            }

            // Build metaData
            var metaData = new HttpEventCollectorEventInfo.Metadata(null, null, "_json", "machinename");

            // Build dynamic properties object object
            dynamic dynProperties = new ExpandoObject(); ;

            dynProperties.Source = name;

            if (exception != null)
            {
                
                dynProperties.Exception = exception;
            }

            // Send the event to splunk
            hecSender.Send(Guid.NewGuid().ToString(), name, null, formatter(state, exception), dynProperties, metaData);
            hecSender.FlushSync();
        }
    }
}
