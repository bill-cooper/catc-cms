using System;
using ceenq.com.Core.Applications;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class SslRedirectServerBlockCreationHandler : IServerBlockCreationHandler
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationDnsNamesService _applicationDnsNamesService;
        public Localizer T { get; set; }
        public SslRedirectServerBlockCreationHandler(IApplicationService applicationService, IApplicationDnsNamesService applicationDnsNamesService)
        {
            _applicationService = applicationService;
            _applicationDnsNamesService = applicationDnsNamesService;
            T = NullLocalizer.Instance;
        }

        public void AddServerBlock(ConfigContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate 'ssl redirect' server block.  The config context was not supplied."), new ArgumentException("Could not generate 'ssl redirect' server block.  The config context was not supplied.", "context"));

            if (!context.Application.TransportSecurity) return;

            var primaryDnsName = _applicationDnsNamesService.PrimaryDnsName(context.Application);
            var appUrl = _applicationService.ApplicationUrl(context.Application).Trim('/');

            var serverBlock = new ServerBlock();
            serverBlock.Port.Add("80");

            //add redirect for primary domain
            serverBlock.DnsNames.Add(primaryDnsName);
            serverBlock.Return = string.Format("301 {0}$request_uri", appUrl);
            context.Config.ServerBlock.Add(serverBlock);

            //if the primary name is a www, then also add a redirect for non-www name
            if (primaryDnsName.StartsWith("www."))
            {
                serverBlock = new ServerBlock();
                serverBlock.Port.Add("80");
                //add non-www subdomain
                serverBlock.DnsNames.Add(string.Format(primaryDnsName.Replace("www.", "")));
                serverBlock.Return = string.Format("301 {0}$request_uri", appUrl);
                context.Config.ServerBlock.Add(serverBlock);
            }
        }
    }
}