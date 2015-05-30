using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace ceenq.com.Logging
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
                .Add(T("Error Log"), "4", item => item.Action("Log", "Admin", new { area = "ceenq.com.Logging" }).Permission(StandardPermissions.SiteOwner));
        }
    }
}