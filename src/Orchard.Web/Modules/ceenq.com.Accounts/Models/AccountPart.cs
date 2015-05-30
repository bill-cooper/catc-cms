using ceenq.com.Core.Accounts;
using Orchard.ContentManagement;

namespace ceenq.com.Accounts.Models {
    public class AccountPart : ContentPart<AccountRecord>, IAccount
    {
        int IContent.Id
        {
            get { return Id; }
        }
        public virtual string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }
        public virtual string DisplayName
        {
            get { return Record.DisplayName; }
            set { Record.DisplayName = value; }
        }
        public virtual string Domain
        {
            get { return Record.Domain; }
            set { Record.Domain = value; }
        }
    }
}