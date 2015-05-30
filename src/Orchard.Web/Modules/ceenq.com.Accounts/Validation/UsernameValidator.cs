using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Orchard;

namespace ceenq.com.Accounts.Validation
{

    public class UsernameValidator :Component, IAccountValidator
    {

        public AccountValidationOrder Order {
            get { return AccountValidationOrder.Default; }
        }
        public IEnumerable<AccountValidationError> Validate(AccountValidationContext context)
        {
            var errors = new List<AccountValidationError>();
            if (context.User == null) return errors;

            if (String.IsNullOrEmpty(context.User.UserName))
            {
                errors.Add(new AccountValidationError("userName", T("You must specify a username.")));
                return errors;
            }

            if (context.User.UserName.Length >= 30)
            {
                errors.Add(new AccountValidationError("userName", T("Username must be less than 30 characters long.")));
            }

            if (context.User.UserName.Length < 4)
            {
                errors.Add(new AccountValidationError("userName", T("Username must be at least four characters long.")));
            }

            if (!Regex.IsMatch(context.User.UserName, @"^[a-zA-Z0-9_\-\.]*$", RegexOptions.IgnoreCase))
            {
                errors.Add(new AccountValidationError("userName", T("Username can only contain numbers, letters, periods and dashes.")));
            }

            return errors;
        }
    }
}