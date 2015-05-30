using System.ComponentModel.DataAnnotations;

namespace ceenq.com.SystemAPI.Models
{
    public class AccountCreate
    {
        [Required]
        [MaxLength(20)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }
    }
}