using Orchard.Events;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerDeletionEventHandler : IEventHandler
    {
        void Deleting(RoutingServerDeletionEventContext context);
        void Deleted(RoutingServerDeletionEventContext context);
    }
    public class RoutingServerDeletionEventContext
    {
        public IRoutingServer RoutingServer { get; set; }
    }
}