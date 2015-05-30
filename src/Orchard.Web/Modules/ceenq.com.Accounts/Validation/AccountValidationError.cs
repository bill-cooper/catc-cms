using Orchard.Localization;

namespace ceenq.com.Accounts.Validation
{
    public class AccountValidationError
    {
        public AccountValidationError(string field, LocalizedString message)
        {
            Field = field;
            Message = message;
        }
        public string Field { get; set; }
        public LocalizedString Message { get; set; }
    }
}