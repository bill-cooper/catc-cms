using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using EMS.PageTitleOverride.Models;

namespace EMS.PageTitleOverride.Handlers {
    public class PageTitleOverrideSettingsPartHandler : ContentHandler {
        public PageTitleOverrideSettingsPartHandler(IRepository<PageTitleOverrideSettingsPartRecord> repository) {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<PageTitleOverrideSettingsPart>("Site"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            // Add in the menu option for the Settings
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Page Title Override")));
        }
    }
}