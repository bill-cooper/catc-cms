using ceenq.com.AwsManagement.Models;
using Orchard;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.Mvc;
using Orchard.Settings;

namespace ceenq.com.AwsManagement.Commands
{
    public class AwsSettingsCommands : DefaultOrchardCommandHandler {
        private readonly IOrchardServices _orchardServices;

        public AwsSettingsCommands(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        [OrchardSwitch]
        public string AccessKey { get; set; }
        [OrchardSwitch]
        public string SecretAccessKey { get; set; }
        [OrchardSwitch]
        public string HostedZoneId { get; set; }


        [CommandName("awssettings")]
        [CommandHelp("awssettings [/AccessKey:accessKey] [/SecretAccessKey:secretAccessKey] [/HostedZoneId:hostedZoneId] \r\n\tPopulate AWS settings.")]
        [OrchardSwitches("AccessKey,SecretAccessKey,HostedZoneId")]
        public void AwsSettings()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<AwsDefaultSettingsPart>();

            if (!string.IsNullOrWhiteSpace(AccessKey)) settings.AccessKey = AccessKey;
            if (!string.IsNullOrWhiteSpace(SecretAccessKey)) settings.SecretAccessKey = SecretAccessKey;
            if (!string.IsNullOrWhiteSpace(HostedZoneId)) settings.HostedZoneId = HostedZoneId;

            Context.Output.WriteLine(T("AWS settings have been populated."));
        }
    }
}