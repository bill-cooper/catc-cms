using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using EMS.PageTitleOverride.Models;

namespace EMS.PageTitleOverride.Handlers {
    public class PageTitleOverridePartHandler : ContentHandler {
        public PageTitleOverridePartHandler(IRepository<PageTitleOverridePartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}