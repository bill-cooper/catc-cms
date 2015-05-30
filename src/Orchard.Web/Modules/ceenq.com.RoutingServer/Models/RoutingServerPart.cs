using System;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Orchard.ContentManagement;

namespace ceenq.com.RoutingServer.Models {
    public class RoutingServerPart : ContentPart<RoutingServerRecord>, IRoutingServer
    {

        public string Name
        {
            get { return Retrieve(x => x.Name); }
            set { Store(x => x.Name, value); }
        }
        public virtual string DnsName
        {
            get { return Retrieve(x => x.DnsName); }
            set { Store(x => x.DnsName, value); }
        }
        public virtual int ConnectionPort
        {
            get { return Retrieve(x => x.ConnectionPort); }
            set { Store(x => x.ConnectionPort, value); }
        }
        public virtual string Username
        {
            get { return Retrieve(x => x.Username); }
            set { Store(x => x.Username, value); }
        }
        public virtual string Password
        {
            get { return Retrieve(x => x.Password); }
            set { Store(x => x.Password, value); }
        }
        public virtual string IpAddress
        {
            get { return Retrieve(x => x.IpAddress); }
            set { Store(x => x.IpAddress, value); }
        }

    }
}