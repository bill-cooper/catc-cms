using System.Collections.Generic;
using System.Text.RegularExpressions;
using Orchard;
using Orchard.Localization;

namespace ceenq.com.Core.Validation
{
    public interface INameValidator : IDependency
    {
        List<string> Validate(string name);
    }

    public class NameValidator : INameValidator
    {
        public NameValidator() {
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
        public List<string> Validate(string name) {
            var errors = new List<string>();
            const string reservedWords = "^(cnq|ceenq|test|development|production|stage|www|content)$";
            const string ludeWords = "^(anal|anus|arrse|ass|asses|asshole|bastard|bitch|blowjob|boner|bukkake|buceta|butthole|clit|cock|cum|cummer|cumming|cums|cumshot|cunt|dick|dickhead|dyke|faggot|fagot|fuck|gangbang|hardcore|homo|horny|hotsex|jackoff|jap|jizz|masterbate|masterbation|masturbate|nigger|orgasm|phonesex|poop|porn|porno|pornography|prick|pussies|pussy|retard|screwing|semen|sex|shemale|shit|shitdick|shite|shithead|shiting|shitings|shits|slut|sluts|smut|snatch|spic|spunk|tit|tits|titties|twat|viagra|xrated|xxx)$";
            const string validName = @"^[a-zA-Z0-9_\-]+$";
            if (Regex.IsMatch(name, reservedWords, RegexOptions.IgnoreCase))
            {
                errors.Add(T("The name that you have chosen is not valid.  Please try a different name.").Text);
                return errors;
            }

            if (Regex.IsMatch(name, ludeWords, RegexOptions.IgnoreCase))
            {
                errors.Add(T("We do not allow lude or sexually oriented content.  Please try a different service provider.").Text);
                return errors;
            }

            if (!Regex.IsMatch(name, validName, RegexOptions.IgnoreCase))
            {
                errors.Add(T("The name can be composed using numbers, letters, dashes, and underscores.  All other characters are invalid.").Text);
                return errors;
            }

            if (Regex.IsMatch(name, @"^\d")) {
                errors.Add(T("The name cannot start with a number.").Text);
                return errors;
            }

            return errors;
        }
    }
}