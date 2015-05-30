using ceenq.com.AwsManagement.Models;
using ceenq.com.Core.Infrastructure.Dns;
using Orchard;
using Orchard.Commands;
using Orchard.ContentManagement;

namespace ceenq.com.AwsManagement.Commands
{
    public class Route53Commands : DefaultOrchardCommandHandler {
        private readonly IDnsManager _dnsManager;
        private readonly IOrchardServices _orchardServices;

        public Route53Commands(IDnsManager dnsManager, IOrchardServices orchardServices)
        {
            _dnsManager = dnsManager;
            _orchardServices = orchardServices;
        }

        [OrchardSwitch]
        public string Host { get; set; }
        [OrchardSwitch]
        public string IpAddress { get; set; }
        [OrchardSwitch]
        public string ToAddress { get; set; }


        [CommandName("route53 createarecord")]
        [CommandHelp("route53 createarecord [/Host:host] [/IpAddress:ipaddress]\r\n\tCreate Route53 A Record.")]
        [OrchardSwitches("Host,IpAddress")]
        public void CreateARecord()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<AwsDefaultSettingsPart>();
            _dnsManager.CreateARecord(Host,IpAddress,settings.AccessKey,settings.SecretAccessKey,settings.HostedZoneId);

            Context.Output.WriteLine(T("Route 53 A Record for host {0} with ip address {1} created.",Host,IpAddress));
        }

        [CommandName("route53 createcname")]
        [CommandHelp("route53 createcname [/Host:host] [/ToAddress:toaddress]\r\n\tCreate Route53 CNAME.")]
        [OrchardSwitches("Host,ToAddress")]
        public void CreateCName()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<AwsDefaultSettingsPart>();
            _dnsManager.CreateCName(Host, ToAddress, settings.AccessKey, settings.SecretAccessKey, settings.HostedZoneId);

            Context.Output.WriteLine(T("Route 53 CNAME for host {0} with to address {1} created.", Host, ToAddress));
        }
    }
}