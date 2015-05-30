using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using ceenq.com.Theme.Admin.Models;

namespace ceenq.com.Theme.Admin
{
    public class AdminThemeSettingsHandler : ContentHandler
    {
        public AdminThemeSettingsHandler(IRepository<AdminThemeSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<AdminThemeSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T("Admin Branding"))
                {
                    Id = "AdminBranding",
                    Position = "20"
                });
        }
    }
}