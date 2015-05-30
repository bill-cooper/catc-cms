using ceenq.com.RoutingServer.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ceenq.com.RoutingServer.Handlers {
    [UsedImplicitly]
    public class RoutingServerPartHandler : ContentHandler {

        public RoutingServerPartHandler(IRepository<RoutingServerRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<RoutingServerPart>("RoutingServer"));

        }
    }
}