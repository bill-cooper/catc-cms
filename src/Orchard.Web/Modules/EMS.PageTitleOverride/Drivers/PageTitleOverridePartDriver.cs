using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using EMS.PageTitleOverride.Models;
using EMS.PageTitleOverride.Services;
using Orchard.ContentManagement.Handlers;
using Orchard;
using Orchard.UI.Resources;
using System;
using System.Web;

namespace EMS.PageTitleOverride.Drivers {
    public class PageTitleOverridePartDriver : ContentPartDriver<PageTitleOverridePart> {
        private readonly IWorkContextAccessor _wca;

        public PageTitleOverridePartDriver(IWorkContextAccessor wca) {
            _wca = wca;
        }

        protected override string Prefix { get { return "PageTitleOverride"; } }

        protected override DriverResult Display(PageTitleOverridePart part, string displayType, dynamic shapeHelper) {
            HttpContext.Current.Cache.Insert("EMS.PageTitleOverride.PageTitle", part.PageTitle ?? "");
            return null;
        }

        protected override DriverResult Editor(PageTitleOverridePart part, dynamic shapeHelper) {
            return ContentShape("Parts_PageTitleOverride_PageTitleOverride",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/PageTitleOverride.PageTitleOverride",
                                   Model: part,
                                   Prefix: Prefix)
                                );
        }

        protected override DriverResult Editor(PageTitleOverridePart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(PageTitleOverridePart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("PageTitle", part.PageTitle);
        }

        protected override void Importing(PageTitleOverridePart part, ImportContentContext context) {
            part.PageTitle = context.Attribute(part.PartDefinition.Name, "PageTitle");
        }
    }
}