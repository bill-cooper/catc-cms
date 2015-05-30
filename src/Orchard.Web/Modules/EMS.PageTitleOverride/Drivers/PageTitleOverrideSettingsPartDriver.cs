using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using EMS.PageTitleOverride.Models;
using EMS.PageTitleOverride.Services;

namespace EMS.PageTitleOverride.Drivers {
    public class PageTitleOverrideSettingsPartDriver : ContentPartDriver<PageTitleOverrideSettingsPart> {
        private readonly ISignals _signals;

        public PageTitleOverrideSettingsPartDriver(ISignals signals) {
            _signals = signals;
        }

        protected override string Prefix { get { return "PageTitleOverrideSettings"; } }

        protected override DriverResult Editor(PageTitleOverrideSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_PageTitleOverride_PageTitleOverrideSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/PageTitleOverride.PageTitleOverrideSettings",
                                   Model: part,
                                   Prefix: Prefix)
                                ).OnGroup("Page Title Override"); // OnGroup just makes sure it only shows up on Page Title Override settings menu
        }

        protected override DriverResult Editor(PageTitleOverrideSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("EMS.PageTitleOverride.Changed"); // Used to reset the cache in PageTitleOverrideServices
            return Editor(part, shapeHelper);
        }
    }
}