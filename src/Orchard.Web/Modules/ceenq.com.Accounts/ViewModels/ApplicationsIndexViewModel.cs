using System.Collections.Generic;

namespace ceenq.com.Accounts.ViewModels {
    public class ApplicationsIndexViewModel  {
        public string AccountName { get; set; }
        public IList<ApplicationViewModel> Rows { get; set; }
    }
}
