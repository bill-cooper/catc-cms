using System.Web.Routing;
using Orchard.Themes;
using Orchard.UI.Admin;

namespace ceenq.com.Theme.Admin
{
    public class AdminThemeSelector : IThemeSelector
    {
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            if (AdminFilter.IsApplied(context))
            {
                return new ThemeSelectorResult { Priority = 110, ThemeName = "ceenq.com.Theme.Admin" };
            }

            return null;
        }
    }
}