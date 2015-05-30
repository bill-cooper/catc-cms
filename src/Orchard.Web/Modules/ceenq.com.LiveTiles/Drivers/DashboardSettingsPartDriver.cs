using ceenq.com.LiveTiles.Models;
using ceenq.com.LiveTiles.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Navigation.Services;

namespace ceenq.com.LiveTiles.Drivers
{
    public class DashboardSettingsPartDriver : ContentPartDriver<DashboardSettingsPart>
    {
        private readonly IContentManager _contentManager;
        private readonly IMenuService _menuService;

        public DashboardSettingsPartDriver(
            IContentManager contentManager,
            IMenuService menuService)
        {
            _contentManager = contentManager;
            _menuService = menuService;
        }
        protected override string Prefix
        {
            get { return "DashboardSettings"; }
        }


        protected override DriverResult Editor(DashboardSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_DashboardSettings_Edit", () =>
            {
                var model = new DashboardSettingsViewModel
                {
                    CurrentMenuId = part.Menu == null ? -1 : part.Menu.Id,
                    Menus = _menuService.GetMenus()
                };

                return shapeHelper.EditorTemplate(TemplateName: "Parts/DashboardSettings", Model: model, Prefix: Prefix);
            }).OnGroup("Dashboard");
        }
        protected override DriverResult Editor(DashboardSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = new DashboardSettingsViewModel();

            if (updater.TryUpdateModel(model, Prefix, null, null))
            {
                part.Menu = _contentManager.Get(model.CurrentMenuId).Record;
            }

            return Editor(part, shapeHelper).OnGroup("Dashboard");
        }

        protected override void Importing(DashboardSettingsPart part, ImportContentContext context)
        {
            context.ImportAttribute(part.PartDefinition.Name, "Menu", x => part.Menu = context.GetItemFromSession(x).Record);
        }

        protected override void Exporting(DashboardSettingsPart part, ExportContentContext context)
        {
            var menuIdentity = _contentManager.GetItemMetadata(_contentManager.Get(part.Menu.Id)).Identity;
            context.Element(part.PartDefinition.Name).SetAttributeValue("Menu", menuIdentity);
        }
    }
}