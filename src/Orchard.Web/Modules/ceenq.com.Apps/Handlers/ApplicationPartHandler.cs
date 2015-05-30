using ceenq.com.Apps.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ceenq.com.Apps.Handlers
{
    [UsedImplicitly]
    public class ApplicationPartHandler : ContentHandler
    {

        public ApplicationPartHandler(IRepository<ApplicationRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<ApplicationPart>("Application"));
        }
    }
}