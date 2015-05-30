using System.Collections.Generic;
using System.Linq;
using ceenq.com.Accounts.Models;
using ceenq.com.Accounts.Validation;
using ceenq.com.Core.Accounts;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;

namespace ceenq.com.Accounts.Drivers
{
    public class AccountPartDriver : ContentPartDriver<AccountPart>
    {
        private readonly IAccountHelper _accountHelper;
        private readonly IAccountContext _accountContext;
        private readonly IEnumerable<IAccountValidator> _accountValidators;

        public AccountPartDriver(IAccountHelper accountHelper, IAccountContext accountContext, IEnumerable<IAccountValidator> accountValidators)
        {
            _accountHelper = accountHelper;
            _accountContext = accountContext;
            _accountValidators = accountValidators;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix
        {
            get { return "AccountPart"; }
        }

        protected override DriverResult Editor(AccountPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Account_AccountPart",
                () =>
                    shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AccountPart",
                        Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(AccountPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            if (string.IsNullOrWhiteSpace(part.DisplayName))
            {
                updater.AddModelError("DisplayName", T("Please provide an Account Name."));
                return ContentShape("Parts_Account_AccountPart",
                   () =>
                       shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AccountPart",
                           Model: part, Prefix: Prefix));
            }

            var cleanName = _accountHelper.CanonicalAccountName(part.DisplayName);

            part.Name = cleanName;
            part.Domain = string.Format("{0}.{1}", cleanName, _accountContext.AccountDomain); //accountcontext represents the current account, which will be a super domain of the account that is being created


            var accountValidationContext = new AccountValidationContext { Account = part };
            foreach (var accountValidator in _accountValidators.Where(validator => validator.Order == AccountValidationOrder.First))
                foreach (var error in accountValidator.Validate(accountValidationContext))
                {
                    updater.AddModelError(error.Field, error.Message);
                }
            foreach (var accountValidator in _accountValidators.Where(validator => validator.Order != AccountValidationOrder.First))
                foreach (var error in accountValidator.Validate(accountValidationContext))
                {
                    updater.AddModelError(error.Field, error.Message);
                }


            return ContentShape("Parts_Account_AccountPart",
            () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AccountPart",
                    Model: part, Prefix: Prefix));
        }
    }
}