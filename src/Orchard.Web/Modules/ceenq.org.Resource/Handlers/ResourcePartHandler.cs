using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using ceenq.org.Resource.Models;

namespace ceenq.org.Resource.Handlers
{
     [UsedImplicitly]
    public class ResourcePartHandler : ContentHandler
    {
        public ResourcePartHandler(IRepository<ResourcePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}