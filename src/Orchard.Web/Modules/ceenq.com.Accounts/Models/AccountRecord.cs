using Orchard.ContentManagement.Records;

namespace ceenq.com.Accounts.Models {
    public class AccountRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Domain { get; set; }
    }
}