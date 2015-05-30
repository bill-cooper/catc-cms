using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockContext
    {
        public LocationBlockContext(LocationBlock locationBlock, IApplication application, IRoute route, IAccountContext accountContext)
        {
            LocationBlock = locationBlock;
            Application = application;
            Route = route;
            AccountContext = accountContext;
        }
        public LocationBlock LocationBlock { get; private set; }
        public IApplication Application { get; private set; }
        public IRoute Route { get; private set; }
        public IAccountContext AccountContext { get; private set; }
    }

}