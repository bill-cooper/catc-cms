using System.Collections.Generic;
using Orchard;
using Orchard.Users.Services;

namespace ceenq.com.Accounts.Validation
{
    public class UniqueAccountUserValidator : Component, IAccountValidator
    {
        private readonly IUserService _orchardUserService;
        public UniqueAccountUserValidator(IUserService orchardUserService)
        {
            _orchardUserService = orchardUserService;
        }

        public AccountValidationOrder Order {
            get { return AccountValidationOrder.Default; }
        }
        public IEnumerable<AccountValidationError> Validate(AccountValidationContext context)
        {
            var errors = new List<AccountValidationError>();
            if (context.User == null) return errors;

            if (!_orchardUserService.VerifyUserUnicity(context.User.UserName, context.User.Email))
            {
                errors.Add(new AccountValidationError("email", T("User with that username and/or email already exists.")));
            }

            return errors;
        }
    }
}