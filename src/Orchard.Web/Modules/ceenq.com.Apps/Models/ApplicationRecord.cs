using Orchard.ContentManagement.Records;

namespace ceenq.com.Apps.Models
{
    public class ApplicationRecord : ContentPartRecord
    {

        public virtual string Name { get; set; }
        public virtual string AuthenticationRedirect { get; set; }
        public virtual string AccountVerification { get; set; }
        public virtual string ResetPassword { get; set; }
        public virtual string Domain { get; set; }
        public virtual bool SuppressDefaultEndpoint { get; set; }
        public virtual bool TransportSecurity { get; set; }
    }
}