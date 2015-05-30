using ceenq.com.AppRoutingServer.Models;
using ceenq.com.Apps.Models;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Infrastructure.Dns;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Services;
using Orchard.ContentManagement;

namespace ceenq.com.AppRoutingServer
{
    public class ApplicationEventHandler : IApplicationEventHandler
    {
        private readonly IDnsManager _dnsManager;
        private readonly IApplicationDnsNamesService _applicationDnsNamesService;
        private readonly IRoutingServerConfigManager _routingServerConfigManager;
        private readonly IRoutingServerManager _routingServerManager;
        private readonly IMountHelper _mountHelper;
        private readonly IAccountContext _accountContext;

        public ApplicationEventHandler(IDnsManager dnsManager, IRoutingServerConfigManager routingServerConfigProvider, IRoutingServerManager routingServerManager, IApplicationDnsNamesService applicationDnsNamesService, IMountHelper mountHelper, IAccountContext accountContext)
        {
            _dnsManager = dnsManager;
            _routingServerConfigManager = routingServerConfigProvider;
            _routingServerManager = routingServerManager;
            _applicationDnsNamesService = applicationDnsNamesService;
            _mountHelper = mountHelper;
            _accountContext = accountContext;
        }
        public void ApplicationCreating(ApplicationEventContext context)
        {
            var application = (ApplicationPart)context.Application;
            var applicationRoutingServer = application.As<ApplicationRoutingServerPart>();
            //if no ip address association, then create new server
            if (string.IsNullOrWhiteSpace(applicationRoutingServer.IpAddress))
            {
                var routingServer = _routingServerManager.New();
                applicationRoutingServer.IpAddress = routingServer.IpAddress;
            }
        }

        public void ApplicationCreated(ApplicationEventContext context)
        {
            //create dns entry
            var application = (ApplicationPart)context.Application;
            var applicationRoutingServer = application.As<ApplicationRoutingServerPart>();
            var dnsName = _applicationDnsNamesService.PrimaryDnsName(application);
            var routingServer = _routingServerManager.Get(applicationRoutingServer.IpAddress);
            //create an A record if this is not the default routing server
            if(!_routingServerManager.IsDefault(routingServer))
                _dnsManager.CreateARecord(dnsName, applicationRoutingServer.IpAddress);


            _routingServerConfigManager.SaveConfig(application,routingServer);
            //create config
        }

        public void ApplicationUpdating(ApplicationEventContext context)
        {
        }

        public void ApplicationUpdated(ApplicationEventContext context)
        {
            ////TODO: if the name of this application has changed, delete the config so a new  config can be created
            ////TODO: if the application has been switch to another ip address, adjust the dns settings and delete the old config
            ////if (originalApplication.Name != application.Name)
            ////    _proxyConfigProvider.DeleteConfig(originalApplication);

           
            var application = (ApplicationPart)context.Application;
            var applicationRoutingServer = application.As<ApplicationRoutingServerPart>();
            var routingServer = _routingServerManager.Get(applicationRoutingServer.IpAddress);
            var commandClient = _routingServerManager.GetCommandClient(routingServer);
            _mountHelper.EnsureMount(commandClient, _accountContext.Account, _accountContext.ServerAbsoluteBasePath);
            _routingServerConfigManager.SaveConfig(application,routingServer);
        }

        public void ApplicationDeleting(ApplicationEventContext context)
        {
        }

        public void ApplicationDeleted(ApplicationEventContext context)
        {
            //TODO:
            //remove dns entry

            var application = (ApplicationPart)context.Application;
            var routingServer = _routingServerManager.Get(application.As<ApplicationRoutingServerPart>().IpAddress);
            _routingServerConfigManager.DeleteConfig(application,routingServer);

        }
    }
}