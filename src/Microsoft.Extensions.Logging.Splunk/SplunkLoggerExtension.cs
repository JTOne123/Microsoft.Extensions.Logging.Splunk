using System;

namespace Microsoft.Extensions.Logging.Splunk
{
    public static class SplunkLoggerExtension
    {
        public static ILoggerFactory AddSplunk(this ILoggerFactory factory, SplunkConfiguration configuration, string applicationName, string environmentName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            if (string.IsNullOrEmpty(environmentName))
            {
                throw new ArgumentNullException(nameof(environmentName));
            }

            ILoggerProvider provider = new SplunkLoggerProvider((n, l, e) => l >= configuration.MinLevel, configuration);

            factory.AddProvider(provider);

            return factory;
        }

        public static ILoggerFactory AddSplunk(this ILoggerFactory factory, Func<string, LogLevel, Exception, bool> filter, SplunkConfiguration configuration, string applicationName, string environmentName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            if (string.IsNullOrEmpty(environmentName))
            {
                throw new ArgumentNullException(nameof(environmentName));
            }

            ILoggerProvider provider = new SplunkLoggerProvider(filter, configuration);

            factory.AddProvider(provider);

            return factory;
        }

        public static ILoggerFactory AddSplunk(this ILoggerFactory factory, SplunkConfiguration configuration)
        {
            ILoggerProvider provider = new SplunkLoggerProvider((n, l, e) => l >= configuration.MinLevel, configuration);

            factory.AddProvider(provider);

            return factory;
        }

        public static ILoggerFactory AddSplunk(this ILoggerFactory factory, Func<string, LogLevel, Exception, bool> filter, SplunkConfiguration configuration)
        {
            ILoggerProvider provider = new SplunkLoggerProvider(filter, configuration);

            factory.AddProvider(provider);

            return factory;
        }
    }
}
