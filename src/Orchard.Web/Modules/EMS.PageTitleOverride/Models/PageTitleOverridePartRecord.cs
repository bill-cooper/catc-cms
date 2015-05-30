using Orchard.ContentManagement.Records;

namespace EMS.PageTitleOverride.Models {
    public class PageTitleOverridePartRecord : ContentPartRecord {
        public virtual string PageTitle { get; set; }
    }
}