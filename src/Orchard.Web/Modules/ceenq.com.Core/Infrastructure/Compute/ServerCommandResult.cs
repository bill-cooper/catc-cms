namespace ceenq.com.Core.Infrastructure.Compute
{
    public class ServerCommandResult
    {
        private readonly string _message;
        public ServerCommandResult(string message)
        {
            _message = message;
        }
        public string Message
        {
            get { return _message; }
        }
    }
}