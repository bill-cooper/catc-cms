using ceenq.com.Core.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace ceenq.com.Core.Handlers {
    [UsedImplicitly]
    public class CoreSettingsPartHandler : ContentHandler {
        public CoreSettingsPartHandler()
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<CoreSettingsPart>("Site"));
        }
        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T("Core Settings")) {
                    Id = "CoreSettings",
                    Position = "25"
                });
        }
    }
}