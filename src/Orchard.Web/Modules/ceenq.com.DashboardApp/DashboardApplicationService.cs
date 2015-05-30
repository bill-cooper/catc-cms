using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using Orchard;

namespace ceenq.com.DashboardApp
{
    public interface IDashboardApplicationService : IDependency
    {
        IApplication BuildDashboardApplication();
    }

    public class DashboardApplicationService : IDashboardApplicationService
    {

        private readonly IAccountContext _accountContext;
        private readonly IRoutingServerManager _routingServerManager;
        public DashboardApplicationService(IAccountContext accountContext, IRoutingServerManager routingServerManager)
        {
            _accountContext = accountContext;
            _routingServerManager = routingServerManager;
        }

        public IApplication BuildDashboardApplication()
        {
            var defaultRoutingServer = _routingServerManager.GetDefault();
            var application = new ApplicationImpl()
            {
                Name = "dashboard",
                IpAddress = defaultRoutingServer == null ? "" : defaultRoutingServer.IpAddress
            };

            application.Routes.Add(new DefaultRouteImpl
            {
                RequestPattern = "=/",
                PassTo = "https://cdn.rawgit.com/jamesryancooper/cnq-dashboard/master/dist/index.html",
                RouteOrder = 0
            });
            application.Routes.Add(new DefaultRouteImpl
            {
                RequestPattern = "/",
                PassTo = "https://cdn.rawgit.com/jamesryancooper/cnq-dashboard/master/dist/",
                RouteOrder = 1
            });
            application.Routes.Add(new DefaultRouteImpl
            {
                RequestPattern = "~* ^/api/(.*)$",
                PassTo = _accountContext.InternalAbsoluteApiPath
            });
            return application;
        }
    }
}