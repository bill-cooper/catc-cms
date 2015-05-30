using System.ComponentModel.DataAnnotations;

namespace ceenq.com.Accounts.ViewModels {
    public class AccountCreateViewModel  {
        [Required, StringLength(255)]
        public string Name { get; set; }
    }
}
