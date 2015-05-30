using ceenq.com.AzureCloudStorage.Models;
using Orchard;
using Orchard.Commands;
using Orchard.ContentManagement;

namespace ceenq.com.AzureCloudStorage.Commands
{
    public class AzureCloudStorageSettingsCommands : DefaultOrchardCommandHandler {
        private readonly IOrchardServices _orchardServices;

        public AzureCloudStorageSettingsCommands(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        [OrchardSwitch]
        public string StorageAccount { get; set; }
        [OrchardSwitch]
        public string StorageKey { get; set; }


        [CommandName("azurestoragesettings")]
        [CommandHelp("azurestoragesettings [/StorageAccount:storageAccount] [/StorageKey:storageKey]\r\n\tPopulate Azure Storage Account settings.")]
        [OrchardSwitches("StorageAccount,StorageKey")]
        public void AwsSettings()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<CloudStorageSettingsPart>();

            if (!string.IsNullOrWhiteSpace(StorageAccount)) settings.StorageAccount = StorageAccount;
            if (!string.IsNullOrWhiteSpace(StorageKey)) settings.StorageKey = StorageKey;

            Context.Output.WriteLine(T("Azure Storage Account settings have been populated."));
        }
    }
}