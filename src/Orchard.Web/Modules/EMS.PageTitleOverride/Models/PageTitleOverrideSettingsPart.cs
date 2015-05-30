using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace EMS.PageTitleOverride.Models {
    public class PageTitleOverrideSettingsPart : ContentPart<PageTitleOverrideSettingsPartRecord> {
        public bool IsPageTitleSiteNameLast {
            get { return Record.IsPageTitleSiteNameLast; }
            set { Record.IsPageTitleSiteNameLast = value; }
        }

        public bool IsPageTitleHideSiteName {
            get { return Record.IsPageTitleHideSiteName; }
            set { Record.IsPageTitleHideSiteName = value; }
        }
    }
}
