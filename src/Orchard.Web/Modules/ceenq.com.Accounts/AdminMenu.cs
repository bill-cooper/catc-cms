using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.Security;

namespace ceenq.com.Accounts {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(T("Accounts"), "4", item => item.Action("Index", "Admin", new { area = "ceenq.com.Accounts" }).Permission(StandardPermissions.SiteOwner));
        }
    }
}
