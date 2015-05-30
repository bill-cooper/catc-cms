using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;

namespace ceenq.com.Theme.Admin
{
    public class AlterLeftNav: FilterProvider, IResultFilter
    {
        private readonly INavigationManager _navigationManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly dynamic _shapeFactory;

        public AlterLeftNav(INavigationManager navigationManager,
            IWorkContextAccessor workContextAccessor,
            IShapeFactory shapeFactory)
        {
            _navigationManager = navigationManager;
            _workContextAccessor = workContextAccessor;
            _shapeFactory = shapeFactory;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // should only run on a full view rendering result
            if (!(filterContext.Result is ViewResult))
            {
                return;
            }

            WorkContext workContext = _workContextAccessor.GetContext(filterContext);

            const string menuName = "admin";
            if (!AdminFilter.IsApplied(filterContext.RequestContext))
            {
                return;
            }

            var menuItems = _navigationManager.BuildMenu(menuName).ToList();


            //These are the items that are on the left menu that should go in the settings menu
            var settingsMenuItems = menuItems
                .Where(m => 
                    m.Text.Text == "Settings"
                    || m.Text.Text == "Import/Export"
                    || m.Text.Text == "Reports"
                    || m.Text.Text == "Users"
                    || m.Text.Text == "Workflows"
                    || m.Text.Text == "Themes"
                    || m.Text.Text == "Modules"
                    || m.Text.Text == "Widgets"
                    || m.Text.Text == "Navigation"
                    || m.Text.Text == "Content Definition"
                    || m.Text.Text == "Queries"
                    || m.Text.Text == "Custom Forms"
                    || m.Text.Text == "Templates"
                    || m.Text.Text == "Jobs Queue"
                    ).ToList();
            settingsMenuItems.ForEach(m => menuItems.Remove(m));
            

            // adding query string parameters
            var routeData = new RouteValueDictionary(filterContext.RouteData.Values);
            var queryString = workContext.HttpContext.Request.QueryString;
            if (queryString != null)
            {
                foreach (var key in from string key in queryString.Keys where key != null && !routeData.ContainsKey(key) let value = queryString[key] select key)
                {
                    routeData[key] = queryString[key];
                }
            }

            // Set the currently selected path
            Stack<MenuItem> selectedPath = NavigationHelper.SetSelectedPath(menuItems, workContext.HttpContext.Request, routeData);

            // Populate main nav
            dynamic menuShape = _shapeFactory.Menu().MenuName(menuName);
            NavigationHelper.PopulateMenu(_shapeFactory, menuShape, menuShape, menuItems);

            // Add any know image sets to the main nav
            IEnumerable<string> menuImageSets = _navigationManager.BuildImageSets(menuName);
            if (menuImageSets != null && menuImageSets.Any())
                menuShape.ImageSets(menuImageSets);

            workContext.Layout.LeftMenu.Add(menuShape);


            dynamic settingsMenuShape = _shapeFactory.SettingsMenu().MenuName(menuName);
            NavigationHelper.PopulateMenu(_shapeFactory, settingsMenuShape, settingsMenuShape, settingsMenuItems);

            if (menuImageSets != null && menuImageSets.Any())
                menuShape.ImageSets(menuImageSets);

            workContext.Layout.TopRight.Add(settingsMenuShape);

        }

        public void OnResultExecuted(ResultExecutedContext filterContext) { }

    }
}