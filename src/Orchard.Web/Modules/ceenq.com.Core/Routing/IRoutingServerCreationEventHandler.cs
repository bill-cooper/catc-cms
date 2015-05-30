using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orchard.Events;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerCreationEventHandler : IEventHandler
    {
        void Creating(RoutingServerCreationEventContext context);
        void Created(RoutingServerCreationEventContext context);
        void PostCreated(RoutingServerCreationEventContext context);
    }

    public class RoutingServerCreationEventContext
    {
        public IRoutingServer RoutingServer { get; set; }
    }
}
