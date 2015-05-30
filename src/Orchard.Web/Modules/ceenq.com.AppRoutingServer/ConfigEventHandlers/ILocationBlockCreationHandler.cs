using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface ILocationBlockCreationHandler : IEventHandler
    {
        void AddLocationBlock(ServerBlockContext context);
    }
}
