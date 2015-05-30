using ceenq.com.Apps.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace ceenq.com.Apps.Handlers {
    [UsedImplicitly]
    public class UserApplicationsPartHandler : ContentHandler {
        public UserApplicationsPartHandler(IRepository<UserApplicationsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<UserApplicationsPart>("User"));
        }
    }
}