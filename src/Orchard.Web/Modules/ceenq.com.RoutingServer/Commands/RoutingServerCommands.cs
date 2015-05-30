using ceenq.com.Core.Infrastructure.Dns;
using ceenq.com.Core.Routing;
using Orchard.Commands;

namespace ceenq.com.RoutingServer.Commands
{
    public class RoutingServerCommands : DefaultOrchardCommandHandler
    {
        private readonly IRoutingServerManager _routingServerManager;
        private readonly IDnsManager _dnsManager;
        public RoutingServerCommands(IRoutingServerManager routingServerManager, IDnsManager dnsManager)
        {
            _routingServerManager = routingServerManager;
            _dnsManager = dnsManager;
        }

        [OrchardSwitch]
        public string DefaultDomain { get; set; }

        [CommandName("routingserver create")]
        [CommandHelp(
            "routingserver create [/DefaultDomain:defaultDomain]\r\n\t" +
            "Creates a routing server and creates the A record for the domain")]
        [OrchardSwitches("DefaultDomain")]
        public void Create()
        {
            var routingServer = _routingServerManager.New();
            if (!string.IsNullOrWhiteSpace(DefaultDomain))
            {
                var wildcardDomain = string.Format("*.{0}", DefaultDomain);
                _dnsManager.CreateARecord(wildcardDomain, routingServer.IpAddress);
            }

        }
    }
}