using System;
using Orchard.Logging;

namespace ceenq.com.Tests
{
    public class FakeLogger : ILogger
    {
        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            LogException = exception;
            LogFormat = format;
            LogArgs = args;
        }

        public Exception LogException { get; set; }
        public string LogFormat { get; set; }
        public object[] LogArgs { get; set; }

        public string Message {
            get { return string.Format(LogFormat, LogArgs); }
        }
    }
}
