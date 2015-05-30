using Orchard.ContentManagement;

namespace ceenq.com.AzureCloudStorage.Models
{
    public class CloudStorageSettingsPart : ContentPart
    {

        public string StorageAccount
        {
            get { return this.Retrieve(x => x.StorageAccount); }
            set { this.Store(x => x.StorageAccount, value); }
        }
        public string StorageKey
        {
            get { return this.Retrieve(x => x.StorageKey); }
            set { this.Store(x => x.StorageKey, value); }
        }
    }
}