using System.Web.Routing;
using Orchard.Themes;
using Orchard.UI.Admin;

namespace ceenq.org.Theme.HarAdm
{
    public class AdminThemeSelector : IThemeSelector
    {
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            if (AdminFilter.IsApplied(context))
            {
                return new ThemeSelectorResult { Priority = 120, ThemeName = "ceenq.org.Theme.HarAdm" };
            }

            return null;
        }
    }
}