using System.Collections.Generic;
using ceenq.com.Accounts.Models;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Models;
using Orchard.Security;

namespace ceenq.com.Accounts.ViewModels {
    public class UserAccountViewModel {
        public UserAccountViewModel() {
            Accounts = new List<IAccount>();
        }
        public List<IAccount> Accounts { get; set; }
        public IUser User { get; set; }
        public string Account { get; set; }
    }
}