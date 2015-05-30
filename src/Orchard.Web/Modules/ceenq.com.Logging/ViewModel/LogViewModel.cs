using System.Collections.Generic;

namespace ceenq.com.Logging.ViewModel
{
    public class LogViewModel
    {
        public int? Level { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }

    public class LogEntryCompare : IEqualityComparer<LogViewModel>
    {
        public bool Equals(LogViewModel x, LogViewModel y)
        {
            return x.Timestamp == y.Timestamp
                    && x.Message == y.Message
                ;
        }
        public int GetHashCode(LogViewModel item)
        {
            return (item.Timestamp + item.Message).GetHashCode();
        }
    }
}