using Orchard.ContentManagement.Records;

namespace ceenq.com.AwsManagement.Models
{
    public class AwsSettingsRecord : ContentPartRecord 
    {
        public virtual string HostedZoneId { get; set; }
        public virtual string AccessKey { get; set; }
        public virtual string SecretAccessKey { get; set; }
    }
}