using ceenq.com.Core.Models;
using Orchard.Events;

namespace ceenq.com.Core.Accounts
{
    public interface IAccountDeletionEventHandler: IEventHandler
    {
        void AccountDeleting(AccountDeletionEventContext context);
        void AccountDeleted(AccountDeletionEventContext context);
    }
    public class AccountDeletionEventContext
    {
        public IAccount Account { get; set; }
    }
}