using System.Collections.Generic;
using Orchard.ContentManagement;

namespace ceenq.com.LiveTiles.ViewModels
{
    public class DashboardSettingsViewModel
    {
        public IEnumerable<ContentItem> Menus { get; set; }
        public int CurrentMenuId { get; set; }
    }
}