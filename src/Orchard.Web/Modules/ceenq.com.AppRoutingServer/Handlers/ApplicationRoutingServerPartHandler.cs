using ceenq.com.AppRoutingServer.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ceenq.com.AppRoutingServer.Handlers {
    [UsedImplicitly]
    public class ApplicationRoutingServerPartHandler : ContentHandler
    {
        public ApplicationRoutingServerPartHandler(IRepository<ApplicationRoutingServerRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<ApplicationRoutingServerPart>("Application"));
        }
    }
}