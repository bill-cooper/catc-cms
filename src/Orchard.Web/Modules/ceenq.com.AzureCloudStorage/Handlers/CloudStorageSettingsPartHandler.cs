using ceenq.com.AzureCloudStorage.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace ceenq.com.AzureCloudStorage.Handlers {
    [UsedImplicitly]
    public class CloudStorageSettingsPartHandler : ContentHandler {
        public CloudStorageSettingsPartHandler()
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<CloudStorageSettingsPart>("Site"));
        }
        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T("Cloud Storage Settings")) {
                    Id = "CloudStorageSettings",
                    Position = "25"
                });
        }
    }
}