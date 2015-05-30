using System;
using System.Text;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using ceenq.org.Services.Models;

namespace ceenq.org.Services.Handlers {
    [UsedImplicitly]
    public class ESVBibleServiceSettingsPartHandler : ContentHandler {
        public ESVBibleServiceSettingsPartHandler(IRepository<ESVBibleServiceSettingsPartRecord> repository) {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<ESVBibleServiceSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("ESV Bible Service")));
        }
    }
}