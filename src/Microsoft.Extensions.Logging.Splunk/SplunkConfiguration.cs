using System;

namespace Microsoft.Extensions.Logging.Splunk
{
    /// <summary>
    /// 
    /// </summary>
    public class SplunkConfiguration
    {
        public Uri ServerUrl { get; set; }
        public string Token { get; set; }
        public int RetriesOnError { get; set; } = 0;
        public bool IgnoreSslErrors { get; set; } = false;
        public LogLevel MinLevel { get; set; } = LogLevel.Information;
    }
}
