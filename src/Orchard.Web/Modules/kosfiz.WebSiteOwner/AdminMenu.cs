using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Localization;
using Orchard.Security;

namespace kosfiz.WebSiteOwner
{
    public class AdminMenu: INavigationProvider
    {
        public Localizer T { get; set; }

        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("WebSiteOwner"), "19", menu=>menu.Add(T("WebSiteOwner"), "0", item=>item.Action("Index", "Admin", new {area = "kosfiz.WebSiteOwner"}))); 
        }

        public string MenuName
        {
            get { return "admin"; }
        }
    }
}