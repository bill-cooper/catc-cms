using ceenq.com.AwsManagement.Models;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.AwsManagement.Drivers
{
    public class AwsSettingsPartDriver : ContentPartDriver<AwsSettingsPart>
    {
        private readonly IOrchardServices _orchardServices;
        public AwsSettingsPartDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        protected override string Prefix
        {
            get { return "AwsSettingsPart"; }
        }

        protected override DriverResult Editor(AwsSettingsPart part, dynamic shapeHelper)
        {
            //Only populate defaults if this is the creation of the content item and not an edit
            // this is identified as an Add because version = 0
            if (part.ContentItem.Version == 0)
            {
                var settings = _orchardServices.WorkContext.CurrentSite.As<AwsDefaultSettingsPart>();
                if (settings != null)
                {
                    part.HostedZoneId = settings.HostedZoneId;
                    part.AccessKey = settings.AccessKey;
                    part.SecretAccessKey = settings.SecretAccessKey;
                }
            }
            return ContentShape("Parts_Account_AwsSettingsPart",
                () =>
                    shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AwsSettingsPart",
                        Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(AwsSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return ContentShape("Parts_Account_AwsSettingsPart",
            () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AwsSettingsPart",
                    Model: part, Prefix: Prefix));
        }
    }
}