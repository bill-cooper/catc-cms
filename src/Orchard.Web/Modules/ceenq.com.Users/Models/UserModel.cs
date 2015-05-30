using System.Collections.Generic;

namespace ceenq.com.Users.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Applications = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<string> Applications { get; set; }
        public bool IsAccountAdmin { get; set; }
    }

}