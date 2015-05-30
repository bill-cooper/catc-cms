using System;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace ceenq.com.DashboardApp
{
    public class RoutingServerCreatedDashboardAppSeeder : Component, IRoutingServerCreationEventHandler
    {
        private readonly IDashboardApplicationService _dashboardApplicationService;
        private readonly IRoutingServerManager _routingServerManager;
        private readonly IRoutingServerConfigManager _routingServerConfigManager;
        private readonly INotifier _notifier;
        public RoutingServerCreatedDashboardAppSeeder(IRoutingServerManager routingServerManager, IRoutingServerConfigManager routingServerConfigManager, INotifier notifier, IDashboardApplicationService dashboardApplicationService)
        {
            _routingServerManager = routingServerManager;
            _routingServerConfigManager = routingServerConfigManager;
            _notifier = notifier;
            _dashboardApplicationService = dashboardApplicationService;
        }

        public void PostCreated(RoutingServerCreationEventContext context)
        {
            try
            {
                // we only want to seed the dashboard on the default routing server
                if (!_routingServerManager.IsDefault(context.RoutingServer)) return;

                var application = _dashboardApplicationService.BuildDashboardApplication();

                _routingServerConfigManager.SaveConfig(application, context.RoutingServer);
                _notifier.Information(T("The dashboard app had been configured for Routing Server {0}",
                    context.RoutingServer.Name));
            }
            catch (Exception ex)
            {
                _notifier.Error(T("Failed to configure dashboard app  for Routing Server {0}",context.RoutingServer.Name));
                Logger.Error(ex, "Failed to configure dashboard app  for Routing Server {0}", context.RoutingServer.Name);
                throw new OrchardException(T("Failed to configure dashboard app  for Routing Server {0}", context.RoutingServer.Name), ex);
            }

        }
        public void Creating(RoutingServerCreationEventContext context)
        {
        }

        public void Created(RoutingServerCreationEventContext context)
        {
        }


    }
}