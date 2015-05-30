using System.ComponentModel.DataAnnotations;

namespace ceenq.com.Accounts.ViewModels {
    public class AccountEditViewModel  {
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }

    }
}
