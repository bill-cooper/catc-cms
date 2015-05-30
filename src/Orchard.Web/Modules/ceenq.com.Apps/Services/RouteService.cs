using ceenq.com.Core.Accounts;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Utility;

namespace ceenq.com.Apps.Services
{
    public class RouteService : IRouteService
    {
        private readonly IAccountContext _accountContext;
        public RouteService(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public bool RoutesToLocalResource(IRoute route)
        {
            var path = route.PassTo.Trim();
            //if this route starts with a module path token, then this is a local resource
            if (path.StartsWith("$")) return false;
            if (UrlHelper.IsValidRelativeUrl(path)) return true;
            return false;
        }

        public bool RoutesToModuleResource(IRoute route)
        {
            var path = route.PassTo.Trim();
            //if this route starts with a module path token, then this is a local resource
            if (path.StartsWith("$")) return true;
            return false;
        }
        public bool RoutesToAccountBaseUrl(IRoute route)
        {
            var path = route.PassTo.Trim();
            if (path.StartsWith(_accountContext.AbsoluteAccountBaseUrl)) return true;
            return false;
        }

        public bool RoutesToInternalResource(IRoute route)
        {
            return RoutesToLocalResource(route) || RoutesToModuleResource(route) || RoutesToAccountBaseUrl(route);
        }
        public bool RoutesToExternalResource(IRoute route)
        {
            var path = route.PassTo.Trim();
            if (UrlHelper.IsValidAbsoluteUrl(path)) return true;
            return false;
        }
    }
}