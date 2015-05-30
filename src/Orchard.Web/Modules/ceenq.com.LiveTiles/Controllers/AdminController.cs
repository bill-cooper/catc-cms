using System.Web.Mvc;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.UI.Admin;
using Orchard.Core.Navigation.Services;
using Orchard.UI.Navigation;
using Orchard.Mvc;
using Orchard;
using Orchard.Core.Title.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Routing;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Navigation.ViewModels;
using Orchard.Localization;
using Orchard.Utility.Extensions;
using Orchard.DisplayManagement;
using ceenq.com.LiveTiles.Models;

namespace ceenq.com.LiveTiles.Controllers
{
    public class AdminController : Controller
    {

        private readonly IMenuService _menuService;
        private readonly INavigationManager _navigationManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IOrchardServices _orchardServices;

        public AdminController(IMenuService menuService,
            INavigationManager navigationManager,
            IWorkContextAccessor workContextAccessor,
            IOrchardServices orchardServices,
            IShapeFactory shapeFactory)
        {
            _menuService = menuService;
            _navigationManager = navigationManager;
            _workContextAccessor = workContextAccessor;
            _orchardServices = orchardServices;
            ShapeHelper = shapeFactory;
        }

        dynamic ShapeHelper { get; set; }

        public ActionResult Index()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DashboardSettingsPart>();

            if (settings.Menu == null)
                return null;
            
            var menu = _menuService.GetMenu(settings.Menu.Id);

            if (menu == null)
                return null;
            
            var menuName = menu.As<TitlePart>().Title.HtmlClassify();
            var currentCulture = _workContextAccessor.GetContext().CurrentCulture;
            var menuItems = _navigationManager.BuildMenu(menu);
            var localized = new List<MenuItem>();
            foreach (var menuItem in menuItems)
            {
                // if there is no associated content, it as culture neutral
                if (menuItem.Content == null)
                {
                    localized.Add(menuItem);
                }

                // if the menu item is culture neutral or of the current culture
                else if (String.IsNullOrEmpty(menuItem.Culture) || String.Equals(menuItem.Culture, currentCulture, StringComparison.OrdinalIgnoreCase))
                {
                    localized.Add(menuItem);
                }
            }

            menuItems = localized;

            var request = _workContextAccessor.GetContext().HttpContext.Request;
            var routeData = request.RequestContext.RouteData;
            var selectedPath = NavigationHelper.SetSelectedPath(menuItems, request, routeData);
            var menuShape = ShapeHelper.Menu();
            
            menuShape.MenuName(menuName);
            menuShape.ContentItem(menu);

            NavigationHelper.PopulateMenu(ShapeHelper, menuShape, menuShape, menuItems);


            return new ShapeResult(this, ShapeHelper.Parts_LiveTiles_Menu(Menu: menuShape));

        }
    }
}