using System.Web.Routing;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace ceenq.org.Resource
{
    public class AdminMenu : INavigationProvider
    {

        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Sermons"), "4",
                        menu => menu
                            .Add(T("List"), "1.0", item => item.Action("List", "Admin", new { area = "Contents", id = "Sermon" }))
                            .Add(T("Create"), "1.0", item => item.Action("Create", "Admin", new { area = "Contents", id = "Sermon" }))
                        );
        }


    }
}