using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.Workflow
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes()) routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = 1,
                                                     Route = new Route(
                                                         "admin/workflows/{action}/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Workflow"},
                                                                                      {"controller", "admin"},
                                                                                        {"action", "index"},
                                                                                        {"id", ""}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Workflow"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}