using Orchard.Events;

namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IServerCommandEventHandler : IEventHandler
    {
        void CommandExecuted(ServerCommandContext context);
    }


    public class ServerCommandContext
    {
        public IServerCommand Command { get; set; }
        public string Message { get; set; }
    }


}
