using ceenq.com.RoutingServer.Models;
using Orchard.ContentManagement.Records;

namespace ceenq.com.AppRoutingServer.Models
{
    public class ApplicationRoutingServerRecord : ContentPartRecord
    {
        public virtual string IpAddress { get; set; }
    }
}