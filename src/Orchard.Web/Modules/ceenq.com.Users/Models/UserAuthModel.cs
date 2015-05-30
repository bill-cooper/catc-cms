using System.ComponentModel.DataAnnotations;

namespace ceenq.com.Users.Models
{
    public class UserAuthModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool RememberMe { get; set; }
        public string Account { get; set; }
    }
}