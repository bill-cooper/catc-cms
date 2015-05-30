using Orchard.Events;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public interface IServerBlockAdjustHandler : IEventHandler
    {
        void AdjustServerBlock(ConfigContext context);
    }

}
