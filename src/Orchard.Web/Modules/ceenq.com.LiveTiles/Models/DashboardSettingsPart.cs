using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace ceenq.com.LiveTiles.Models
{
    public class DashboardSettingsPart : ContentPart<DashboardSettingsPartRecord> 
    {
        public ContentItemRecord Menu
        {
            get { return Record.Menu; }
            set { Record.Menu = value; }
        }
    }

    public class DashboardSettingsPartRecord : ContentPartRecord 
    {
        public virtual ContentItemRecord Menu { get; set; }
    }
}