using System;
using System.Linq;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class PrimaryServerBlockCreationHandler : IServerBlockCreationHandler
    {
        private readonly IApplicationDnsNamesService _applicationDnsNamesService;
        private readonly ILocationBlockCreationHandler _locationBlockCreationHandlers;
        private readonly IServerBlockConfigurationHandler _serverBlockConfigurationHandler;
        private readonly IAccountContext _accountContext;
        public Localizer T { get; set; }
        public PrimaryServerBlockCreationHandler(IApplicationDnsNamesService applicationDnsNamesService, IAccountContext accountContext, IServerBlockConfigurationHandler serverBlockConfigurationHandler, ILocationBlockCreationHandler locationBlockCreationHandler)
        {
            _applicationDnsNamesService = applicationDnsNamesService;
            _locationBlockCreationHandlers = locationBlockCreationHandler;
            _accountContext = accountContext;
            _serverBlockConfigurationHandler = serverBlockConfigurationHandler;
            T = NullLocalizer.Instance;
        }

        public void AddServerBlock(ConfigContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate primary server block.  The config context was not supplied."), new ArgumentException("Could not generate primary server block.  The config context was not supplied.", "context"));

            var serverBlock = new ServerBlock();

            var application = context.Application;

            if (_applicationDnsNamesService.DnsNames(application).Count == 0)
                throw new ConfigGenerationException(T("Could not generate primary server block.  No DNS names are associated with application named '{0}'", application.Name));

            if (application.TransportSecurity)
            {
                serverBlock.Port.Add("443 ssl");
                serverBlock.Port.Add("[::]:443 ipv6only=on");
                serverBlock.SslCertLocation = "ssl/bundle.cer";
                serverBlock.SslCertKeyLocation = "ssl/key.pem";
                serverBlock.SslPreferServerCiphers = "on";
                serverBlock.Header.Add("Strict-Transport-Security \"max-age=31536000; includeSubdomains\"");
                serverBlock.SslProtocols.Add("TLSv1");
                serverBlock.SslProtocols.Add("TLSv1.1");
                serverBlock.SslProtocols.Add("TLSv1.2");
                serverBlock.SslCiphers = "'ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-ECDSA-AES256-GCM-SHA384:DHE-RSA-AES128-GCM-SHA256:DHE-DSS-AES128-GCM-SHA256:kEDH+AESGCM:ECDHE-RSA-AES128-SHA256:ECDHE-ECDSA-AES128-SHA256:ECDHE-RSA-AES128-SHA:ECDHE-ECDSA-AES128-SHA:ECDHE-RSA-AES256-SHA384:ECDHE-ECDSA-AES256-SHA384:ECDHE-RSA-AES256-SHA:ECDHE-ECDSA-AES256-SHA:DHE-RSA-AES128-SHA256:DHE-RSA-AES128-SHA:DHE-DSS-AES128-SHA256:DHE-RSA-AES256-SHA256:DHE-DSS-AES256-SHA:DHE-RSA-AES256-SHA:AES128-GCM-SHA256:AES256-GCM-SHA384:AES128-SHA256:AES256-SHA256:AES128-SHA:AES256-SHA:AES:CAMELLIA:DES-CBC3-SHA:!aNULL:!eNULL:!EXPORT:!DES:!RC4:!MD5:!PSK:!aECDH:!EDH-DSS-DES-CBC3-SHA:!EDH-RSA-DES-CBC3-SHA:!KRB5-DES-CBC3-SHA'";
            }
            else
            {
                serverBlock.Port.Add("80");
            }

            serverBlock.DnsNames.AddRange(_applicationDnsNamesService.DnsNames(application));

            var serverBlockContext = new ServerBlockContext(serverBlock, context.Application, _accountContext);

            _serverBlockConfigurationHandler.ConfigureServerBlock(serverBlockContext);
            _locationBlockCreationHandlers.AddLocationBlock(serverBlockContext);

            serverBlock.LocationBlocks =
                serverBlock.LocationBlocks.OrderBy(locationBlock => locationBlock.Order).ToList();
            context.Config.ServerBlock.Add(serverBlock);
        }
    }
}