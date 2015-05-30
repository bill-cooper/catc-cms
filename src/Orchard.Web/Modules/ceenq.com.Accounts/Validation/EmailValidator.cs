using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Orchard;
using Orchard.Users.Models;

namespace ceenq.com.Accounts.Validation
{
    public class EmailValidator :Component, IAccountValidator
    {
        public AccountValidationOrder Order {
            get { return AccountValidationOrder.Default; }
        }
        public IEnumerable<AccountValidationError> Validate(AccountValidationContext context)
        {
            var errors = new List<AccountValidationError>();
            if (context.User == null) return errors;

            if (String.IsNullOrEmpty(context.User.Email))
            {
                errors.Add(new AccountValidationError("email",T("You must specify an email address.")));
                return errors;
            }
            if (context.User.Email.Length >= 255)
            {
                errors.Add(new AccountValidationError("email", T("The email address you provided is too long.")));
                return errors;
            }
            if (!Regex.IsMatch(context.User.Email, UserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                errors.Add(new AccountValidationError("email", T("Please specify a valid email address.")));
                return errors;
            }
            return errors;
        }
    }
}