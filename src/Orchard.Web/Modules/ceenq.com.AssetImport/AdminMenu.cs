using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace ceenq.com.AssetImport
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }

        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder
                .Add(T("Asset Import"), "4", item => item.Action("Import", "Admin", new { area = "ceenq.com.AssetImport" }).Permission(StandardPermissions.SiteOwner));
            builder
                .Add(T("Asset Purge"), "4", item => item.Action("Purge", "Admin", new { area = "ceenq.com.AssetImport" }).Permission(StandardPermissions.SiteOwner));
        }
    }
}