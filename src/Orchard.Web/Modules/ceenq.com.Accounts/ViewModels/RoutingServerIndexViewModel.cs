using System.Collections.Generic;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Models;

namespace ceenq.com.Accounts.ViewModels {
    public class RoutingServerIndexViewModel
    {
        public string AccountName { get; set; }
        public IEnumerable<IRoutingServer> Rows { get; set; }
    }
}
