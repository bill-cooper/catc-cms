namespace ceenq.com.Core.Infrastructure.Compute
{
    public class SshConnectionInfo : IServerCommandClientContext
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        private int _connectionTimeout = 10;//seconds
        public int ConnectionTimeout
        {
            get { return _connectionTimeout; }
            set { _connectionTimeout = value; }
        }
    }
}