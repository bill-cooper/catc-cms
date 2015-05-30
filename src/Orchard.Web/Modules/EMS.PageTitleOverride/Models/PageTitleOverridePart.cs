using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace EMS.PageTitleOverride.Models {
    public class PageTitleOverridePart : ContentPart<PageTitleOverridePartRecord> {
        public string PageTitle {
            get { return Record.PageTitle; }
            set { Record.PageTitle = value; }
        }
    }
}
