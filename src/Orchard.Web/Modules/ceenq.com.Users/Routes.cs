using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.Users
{
    public class Routes : IRouteProvider
    {

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = 1,
                                                     Route = new Route(
                                                         "users/account/{action}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Users"},
                                                                                      {"controller", "BackendUser"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Users"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}