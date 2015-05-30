
namespace ceenq.com.Users.Models
{
    public class ResetPasswordModel
    {
        public string Nonce { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}