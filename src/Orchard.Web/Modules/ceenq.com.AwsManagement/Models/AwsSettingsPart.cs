using Orchard.ContentManagement;

namespace ceenq.com.AwsManagement.Models
{
    public class AwsSettingsPart : ContentPart<AwsSettingsRecord>
    {
        public virtual string HostedZoneId
        {
            get { return Record.HostedZoneId; }
            set { Record.HostedZoneId = value; }
        }
        public virtual string AccessKey
        {
            get { return Record.AccessKey; }
            set { Record.AccessKey = value; }
        }
        public virtual string SecretAccessKey
        {
            get { return Record.SecretAccessKey; }
            set { Record.SecretAccessKey = value; }
        }
    }
}