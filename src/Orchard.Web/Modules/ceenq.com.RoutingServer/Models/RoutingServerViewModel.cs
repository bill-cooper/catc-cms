using ceenq.com.Core.Routing;
using Orchard.ContentManagement.Records;

namespace ceenq.com.RoutingServer.Models
{
    public class RoutingServerViewModel
    {
        public virtual string Name { get; set; }
        public virtual string DnsName { get; set; }
        public virtual int ConnectionPort { get; set; }
        public virtual string Username { get; set; }
        public virtual string IpAddress { get; set; }
    }
}