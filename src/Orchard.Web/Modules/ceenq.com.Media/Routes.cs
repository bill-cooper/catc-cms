using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace ceenq.com.Media {
    public class Routes : IRouteProvider {


        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Download/{mediaType}/{mediaId}/{fileName}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Media"},
                                                                                      {"controller", "Download"},
                                                                                      {"action", "Download"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Media"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }

}