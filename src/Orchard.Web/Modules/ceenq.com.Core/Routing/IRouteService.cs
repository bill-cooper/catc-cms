using Orchard;

namespace ceenq.com.Core.Routing
{
    public interface IRouteService : IDependency
    {
        bool RoutesToLocalResource(IRoute route);
        bool RoutesToModuleResource(IRoute route);
        bool RoutesToInternalResource(IRoute route);
        bool RoutesToExternalResource(IRoute route);
    }
}