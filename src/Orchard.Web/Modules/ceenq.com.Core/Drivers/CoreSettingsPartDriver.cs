using ceenq.com.Core.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.Core.Drivers
{
    public class CoreSettingsPartDriver : ContentPartDriver<CoreSettingsPart> {
        private const string TemplateName = "Parts/CoreSettings";

        protected override string Prefix { get { return "CoreSettings"; } }

        protected override DriverResult Editor(CoreSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_CoreSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix))
                    .OnGroup("CoreSettings");
        }

        protected override DriverResult Editor(CoreSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_CoreSettings_Edit", () =>
            {
                    updater.TryUpdateModel(part, Prefix, null, null);

                    return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
                })
                .OnGroup("CoreSettings");
        }
    }
}