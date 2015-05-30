using ceenq.com.AwsManagement.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.AwsManagement.Drivers
{
    public class AwsDefaultSettingsPartDriver : ContentPartDriver<AwsDefaultSettingsPart>
    {
        protected override string Prefix { get { return "AwsDefaultSettings"; } }

        protected override DriverResult Editor(AwsDefaultSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AwsDefaultSettings",
                    () => shapeHelper.EditorTemplate(TemplateName: "Parts.AwsDefaultSettings", Model: part, Prefix: Prefix))
                    .OnGroup("AwsDefaultSettings");
        }

        protected override DriverResult Editor(AwsDefaultSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_AwsDefaultSettings", () =>
            {
                    updater.TryUpdateModel(part, Prefix, null, null);

                    return shapeHelper.EditorTemplate(TemplateName: "Parts.AwsDefaultSettings", Model: part, Prefix: Prefix);
                })
                .OnGroup("AwsDefaultSettings");
        }
    }
}