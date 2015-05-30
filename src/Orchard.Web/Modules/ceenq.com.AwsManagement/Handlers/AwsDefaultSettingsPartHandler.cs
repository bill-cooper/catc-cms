using ceenq.com.AwsManagement.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace ceenq.com.AwsManagement.Handlers {
    [UsedImplicitly]
    public class AwsDefaultSettingsPartHandler : ContentHandler {
        public AwsDefaultSettingsPartHandler()
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<AwsDefaultSettingsPart>("Site"));
        }
        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T("AWS Default Settings")) {
                    Id = "AwsDefaultSettings",
                    Position = "25"
                });
        }
    }
}