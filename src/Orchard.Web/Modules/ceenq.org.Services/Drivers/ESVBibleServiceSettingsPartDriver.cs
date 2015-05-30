using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using ceenq.org.Services.Models;

namespace ceenq.org.Services.Drivers {

    // We define a specific driver instead of using a TemplateFilterForRecord, because we need the model to be the part and not the record.
    // Thus the encryption/decryption will be done when accessing the part's property

    public class ESVBibleServiceSettingsPartDriver : ContentPartDriver<ESVBibleServiceSettingsPart> {
        private const string TemplateName = "Parts/ESVBibleServiceSettings";

        public ESVBibleServiceSettingsPartDriver() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "ESVBibleServiceSettings"; } }

        protected override DriverResult Editor(ESVBibleServiceSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_ESVBibleServiceSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix))
                    .OnGroup("esv bible service");
        }

        protected override DriverResult Editor(ESVBibleServiceSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            return ContentShape("Parts_ESVBibleServiceSettings_Edit", () => {
                    updater.TryUpdateModel(part, Prefix, null, null);
                    return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
                })
                .OnGroup("esv bible service");
        }
    }
}