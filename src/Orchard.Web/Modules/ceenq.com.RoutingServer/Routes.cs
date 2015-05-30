using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.RoutingServer {
    public class Routes : IRouteProvider {


        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Proxy/Log",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.RoutingServer"},
                                                                                      {"controller", "Log"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.RoutingServer"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Proxy/Command",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.RoutingServer"},
                                                                                      {"controller", "Command"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.RoutingServer"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}