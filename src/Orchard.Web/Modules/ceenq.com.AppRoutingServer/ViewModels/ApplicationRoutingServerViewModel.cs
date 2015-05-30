using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ceenq.com.AppRoutingServer.ViewModels
{
    public class ApplicationRoutingServerViewModel
    {
        public ApplicationRoutingServerViewModel()
        {
            RoutingServers = new List<string>();
        }
        public List<string> RoutingServers { get; set; }
        public string SelectedRoutingServer { get; set; }
        public string Application { get; set; }
    }
}