using ceenq.com.Core.Models;
using Orchard;
using Orchard.Commands;
using Orchard.ContentManagement;

namespace ceenq.com.Core.Commands
{
    public class AwsSettingsCommands : DefaultOrchardCommandHandler {
        private readonly IOrchardServices _orchardServices;

        public AwsSettingsCommands(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        [OrchardSwitch]
        public string AccountDomain { get; set; }
        [OrchardSwitch]
        public string ParentTenant { get; set; }


        [CommandName("coresettings")]
        [CommandHelp("coresettings [/AccountDomain:accountDomain] [/ParentTenant:parentTenant] \r\n\tPopulate core settings.")]
        [OrchardSwitches("AccountDomain,ParentTenant")]
        public void AwsSettings()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<CoreSettingsPart>();

            if (!string.IsNullOrWhiteSpace(AccountDomain)) settings.AccountDomain = AccountDomain;
            if (!string.IsNullOrWhiteSpace(ParentTenant)) settings.ParentTenant = ParentTenant;

            Context.Output.WriteLine(T("AWS settings have been populated."));
        }
    }
}