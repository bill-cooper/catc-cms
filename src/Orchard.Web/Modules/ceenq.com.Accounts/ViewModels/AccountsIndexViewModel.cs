using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;

namespace ceenq.com.Accounts.ViewModels {
    public class AccountsIndexViewModel  {
        public AccountsIndexViewModel()
        {
            Rows = new List<AccountViewModel>();
        }
        public IList<AccountViewModel> Rows { get; set; }
    }

    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Domain { get; set; }
        public string DefaultRoutingServer { get; set; }
        public ContentItem ContentItem { get; set; }
        public ShellSettings Tenant { get; set; }
    }
}
