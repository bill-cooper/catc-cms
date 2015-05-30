using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Models;
using Orchard.ContentManagement;

namespace ceenq.com.AppRoutingServer.Models
{
    public class ApplicationRoutingServerPart : ContentPart<ApplicationRoutingServerRecord> , IApplicationRoutingServer
    {
        public string IpAddress
        {
            get { return Retrieve(x => x.IpAddress); }
            set { Store(x => x.IpAddress, value); }
        }
    }
}