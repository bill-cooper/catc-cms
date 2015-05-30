using ceenq.com.AzureManagement.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace ceenq.com.AzureManagement.Handlers {
    [UsedImplicitly]
    public class AzureDefaultSettingsPartHandler : ContentHandler {
        public AzureDefaultSettingsPartHandler()
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<AzureDefaultSettingsPart>("Site"));
        }
        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T("Azure Default Settings")) {
                    Id = "AzureDefaultSettings",
                    Position = "25"
                });
        }
    }
}