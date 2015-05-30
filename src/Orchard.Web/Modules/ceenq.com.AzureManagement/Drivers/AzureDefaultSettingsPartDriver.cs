using ceenq.com.AzureManagement.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.AzureManagement.Drivers
{
    public class AzureDefaultSettingsPartDriver : ContentPartDriver<AzureDefaultSettingsPart>
    {

        protected override string Prefix { get { return "AzureDefaultSettings"; } }

        protected override DriverResult Editor(AzureDefaultSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_AzureDefaultSettings",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.AzureDefaultSettings", Model: part, Prefix: Prefix))
                    .OnGroup("AzureDefaultSettings");
        }

        protected override DriverResult Editor(AzureDefaultSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_AzureDefaultSettings", () =>
            {
                    updater.TryUpdateModel(part, Prefix, null, null);

                    return shapeHelper.EditorTemplate(TemplateName: "Parts.AzureDefaultSettings", Model: part, Prefix: Prefix);
                })
                .OnGroup("AzureDefaultSettings");
        }
    }
}