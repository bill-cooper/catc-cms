using ceenq.com.Core.Accounts;
using Orchard.Security;

namespace ceenq.com.Accounts.Validation
{
    public class AccountValidationContext
    {
        public IUser User { get; set; }
        public IAccount Account { get; set; }
    }
}