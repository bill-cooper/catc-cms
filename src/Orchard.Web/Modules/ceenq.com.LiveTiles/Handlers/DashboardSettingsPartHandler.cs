using ceenq.com.LiveTiles.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace ceenq.com.LiveTiles.Handlers
{
    public class DashboardSettingsPartHandler : ContentHandler {
        public DashboardSettingsPartHandler(IRepository<DashboardSettingsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<DashboardSettingsPart>("Site"));
            T = NullLocalizer.Instance;
        }
 
        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T("Dashboard"))
                {
                    Id = "Dashboard",
                    Position = "20"
                });
        }
    }
}