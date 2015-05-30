using Orchard.Events;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerMaintenanceEventHandler : IEventHandler
    {
        void Restart(RoutingServerMaintenanceEventContext context);
        void PowerOn(RoutingServerMaintenanceEventContext context);
        void PowerOff(RoutingServerMaintenanceEventContext context);
    }
    public class RoutingServerMaintenanceEventContext
    {
        public IRoutingServer RoutingServer { get; set; }
    }
}