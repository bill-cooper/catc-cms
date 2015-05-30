using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface IServerBlockCreationHandler: IEventHandler
    {
        void AddServerBlock(ConfigContext context);
    }
}
