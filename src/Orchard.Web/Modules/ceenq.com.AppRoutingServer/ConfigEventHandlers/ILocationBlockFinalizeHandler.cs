using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface ILocationBlockFinalizeHandler : IEventHandler
    {
        void FinalizeLocationBlock(LocationBlockContext context);
    }
}
