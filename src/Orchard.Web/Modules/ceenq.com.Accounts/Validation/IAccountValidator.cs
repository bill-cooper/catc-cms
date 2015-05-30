using System.Collections.Generic;
using Orchard;

namespace ceenq.com.Accounts.Validation
{
    public enum AccountValidationOrder
    {
        First,
        Default
    }

    public interface IAccountValidator : IDependency
    {
        AccountValidationOrder Order { get;}
        IEnumerable<AccountValidationError> Validate(AccountValidationContext context);
    }
}