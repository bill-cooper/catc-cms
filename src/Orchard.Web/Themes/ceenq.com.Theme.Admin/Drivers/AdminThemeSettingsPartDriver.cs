using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using ceenq.com.Theme.Admin.Models;

namespace ceenq.com.Theme.Admin.Drivers
{
    public class AdminThemeSettingsPartDriver : ContentPartDriver<AdminThemeSettingsPart>
    {
        private const string TemplateName = "Parts/AdminThemeSettings";

        public AdminThemeSettingsPartDriver()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "AdminThemeSettings"; } }

        protected override DriverResult Editor(AdminThemeSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AdminThemeSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix))
                    .OnGroup("AdminBranding");
        }

        protected override DriverResult Editor(AdminThemeSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_AdminThemeSettings_Edit", () =>
            {
                updater.TryUpdateModel(part, Prefix, null, null);
                return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
            })
                .OnGroup("AdminBranding");
        }
    }
}