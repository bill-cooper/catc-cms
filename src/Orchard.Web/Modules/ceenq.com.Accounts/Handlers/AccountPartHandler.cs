using System;
using ceenq.com.Accounts.Models;
using ceenq.com.Core.Accounts;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using Orchard.UI.Notify;

namespace ceenq.com.Accounts.Handlers {
    [UsedImplicitly]
    public class AccountPartHandler : ContentHandler
    {

        private readonly IAccountCreationEventHandler _accountCreationEventHandler;
        private readonly IAccountDeletionEventHandler _accountDeletionEventHandler;
        private readonly ITransactionManager _transactionManager;
        private readonly INotifier _notifier;

        public AccountPartHandler()
        {
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
        public AccountPartHandler(IRepository<AccountRecord> repository, IAccountCreationEventHandler accountCreationEventHandler, IAccountDeletionEventHandler accountDeletionEventHandler, ITransactionManager transactionManager, INotifier notifier)
        {
            _accountCreationEventHandler = accountCreationEventHandler;
            _accountDeletionEventHandler = accountDeletionEventHandler;
            _transactionManager = transactionManager;
            _notifier = notifier;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<AccountPart>("Account"));

            OnUpdated<AccountPart>((ctx, part) =>
            {
                var context = new AccountCreationEventContext { Account = part };
                //The ContentHandler.OnCreated handler is not suitable for triggering the account creation events
                // because it is too early in the content item creation pipeline and the account object properties
                // have not yet been populated from the view
                //therefore the event triggering has ben placed in OnUpdated and the content item version is used to identify this as an account creation
                if (part.ContentItem.Version == 1)
                {
                    try
                    {
                        _accountCreationEventHandler.Initialize(context);
                        _accountCreationEventHandler.Created(context);
                        _accountCreationEventHandler.PostCreation(context);
                    }
                    catch (Exception)
                    {
                        //if anything goes wrong in any of the account creation event handlers, then cancel the account creation
                        _transactionManager.Cancel();
                        _notifier.Error(T("Your Account was NOT created."));
                        throw;
                    }
                }
            });
            OnRemoving<AccountPart>((ctx, part) =>
            {
                var context = new AccountDeletionEventContext { Account = part };

                _accountDeletionEventHandler.AccountDeleting(context);
            });
            OnRemoved<AccountPart>((ctx, part) =>
            {
                var context = new AccountDeletionEventContext { Account = part };

                _accountDeletionEventHandler.AccountDeleted(context);
            });
        }
    }
}