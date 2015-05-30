using System.Web.Routing;
using Orchard.Themes;
using Orchard.UI.Admin;
using ceenq.com.Core.Extensions;

namespace ceenq.com.Theme.LayoutsAdmin
{
    public class AdminThemeSelector : IThemeSelector
    {
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            if (AdminFilter.IsApplied(context) && context.IsEmbedded())
            {
                return new ThemeSelectorResult { Priority = 110, ThemeName = "ceenq.com.Theme.LayoutsAdmin" };
            }

            return null;
        }
    }
}