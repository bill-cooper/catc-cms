using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface ILocationBlockConfigurationHandler : IEventHandler
    {
        void ConfigureLocationBlock(LocationBlockContext context);
    }
}
