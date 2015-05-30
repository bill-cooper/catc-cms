using System;
using System.Collections.Generic;
using ceenq.com.Core.Accounts;
using Orchard;

namespace ceenq.com.Apps.Validation
{
    public interface IApplicationNameValidator : IDependency
    {
        List<string> Validate(string name);
    }

    public class ApplicationNameValidator : Component, IApplicationNameValidator
    {
        private readonly IAccountContext _accountContext;
        public ApplicationNameValidator(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public List<string> Validate(string name) {
            var errors = new List<string>();

            if (string.Compare(name, _accountContext.Account, StringComparison.OrdinalIgnoreCase) == 0)
            {
                errors.Add(T("The application name must be different then the account name.").Text);
                return errors;
            }
            //REFACTOR: The account name and app should not be hard coded.  This should pull from
            // the DashboardApp module setting
            if (name == "dashboard" && !_accountContext.Account.StartsWith("cnq"))
            {
                errors.Add(T("The application cannot be named {0}", name).Text);
                return errors;
            }

            return errors;
        }
    }
}