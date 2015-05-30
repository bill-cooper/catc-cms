using Orchard.ContentManagement;

namespace ceenq.com.AwsManagement.Models
{
    public class AwsDefaultSettingsPart : ContentPart
    {
        public  string HostedZoneId
        {
            get { return this.Retrieve(x => x.HostedZoneId); }
            set { this.Store(x => x.HostedZoneId, value); }
        }
        public  string AccessKey
        {
            get { return this.Retrieve(x => x.AccessKey); }
            set { this.Store(x => x.AccessKey, value); }
        }
        public  string SecretAccessKey
        {
            get { return this.Retrieve(x => x.SecretAccessKey); }
            set { this.Store(x => x.SecretAccessKey, value); }
        }
    }
}