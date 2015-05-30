using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface IServerBlockConfigurationHandler : IEventHandler
    {
        void ConfigureServerBlock(ServerBlockContext context);
    }
}
