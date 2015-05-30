using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface ILocationBlockAdjustHandler : IEventHandler
    {
        void AdjustLocationBlock(LocationBlockContext context);
    }
}
