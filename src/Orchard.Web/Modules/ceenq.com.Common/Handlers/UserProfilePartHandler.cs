using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using ceenq.com.Common.Models;

namespace ceenq.com.Common.Handlers
{
    public class UserProfilePartHandler : ContentHandler
    {
        public UserProfilePartHandler(IRepository<UserProfilePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}