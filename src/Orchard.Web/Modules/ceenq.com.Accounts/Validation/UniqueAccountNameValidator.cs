using System.Collections.Generic;
using ceenq.com.Core.Accounts;
using Orchard;

namespace ceenq.com.Accounts.Validation
{

    public class UniqueAccountNameValidator :Component, IAccountValidator
    {
        private readonly IAccountManager _accountManager;
        public UniqueAccountNameValidator(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public AccountValidationOrder Order {
            get { return AccountValidationOrder.First; }
        }
        public IEnumerable<AccountValidationError> Validate(AccountValidationContext context)
        {
            var errors = new List<AccountValidationError>();

            if (string.IsNullOrWhiteSpace(context.Account.Name))
            {
                errors.Add(new AccountValidationError("name", T("The account name cannot be empty.")));
                return errors;
            }

            if (string.IsNullOrWhiteSpace(context.Account.Name) || _accountManager.GetAccountByName(context.Account.Name) != null)
                errors.Add(new AccountValidationError("name", T("An account named '{0}' already exists", context.Account.Name)));

            return errors;
        }
    }
}