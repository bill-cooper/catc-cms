using Orchard.ContentManagement.Records;

namespace EMS.PageTitleOverride.Models {
    public class PageTitleOverrideSettingsPartRecord : ContentPartRecord {
        public virtual bool IsPageTitleSiteNameLast { get; set; }
        public virtual bool IsPageTitleHideSiteName { get; set; }
    }
}