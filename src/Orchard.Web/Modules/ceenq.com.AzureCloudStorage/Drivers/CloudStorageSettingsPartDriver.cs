using ceenq.com.AzureCloudStorage.Models;
using ceenq.com.Core.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.AzureCloudStorage.Drivers
{
    public class CloudStorageSettingsPartDriver : ContentPartDriver<CloudStorageSettingsPart> {
        private const string TemplateName = "Parts/CloudStorageSettings";

        protected override string Prefix { get { return "CloudStorageSettings"; } }

        protected override DriverResult Editor(CloudStorageSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_CloudStorageSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix))
                    .OnGroup("CloudStorageSettings");
        }

        protected override DriverResult Editor(CloudStorageSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_CloudStorageSettings_Edit", () =>
            {
                    updater.TryUpdateModel(part, Prefix, null, null);

                    return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
                })
                .OnGroup("CloudStorageSettings");
        }
    }
}