using ceenq.com.Core.Models;
using Orchard.Events;
using Orchard.Security;

namespace ceenq.com.Core.Accounts
{
    public interface IAccountCreationEventHandler : IEventHandler
    {
        void Initialize(AccountCreationEventContext context);
        void Created(AccountCreationEventContext context);
        void PostCreation(AccountCreationEventContext context);
    }



    public class AccountCreationEventContext
    {
        public IAccount Account { get; set; }
        public IUser User { get; set; }
    }
}
