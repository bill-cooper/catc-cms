using Orchard.ContentManagement;

namespace ceenq.com.Core.Models
{
    public class CoreSettingsPart : ContentPart
    {
        public string AccountDomain
        {
            get { return this.Retrieve(x => x.AccountDomain); }
            set { this.Store(x => x.AccountDomain, value); }
        }

        public string ParentTenant
        {
            get { return this.Retrieve(x => x.ParentTenant); }
            set { this.Store(x => x.ParentTenant, value); }
        }

    }
}