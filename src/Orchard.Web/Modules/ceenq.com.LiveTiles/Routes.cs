using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.LiveTiles
{
    public class Routes : IRouteProvider {
        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = 100,
                                                     Route = new Route(
                                                         "Admin",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.LiveTiles"},
                                                                                      {"controller", "admin"},
                                                                                      {"action", "index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.LiveTiles"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}