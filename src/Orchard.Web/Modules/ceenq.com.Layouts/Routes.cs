using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.Layouts
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
                                                         "Admin/ContentTypes/{action}/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Layouts"},
                                                                                      {"controller", "Admin"},
                                                                                        {"action", "index"},
                                                                                        {"id", ""}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Layouts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                           new RouteDescriptor {
                                                     Priority = 1,
                                                     Route = new Route(
                                                         "Projector/filter/{action}/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Layouts"},
                                                                                      {"controller", "Filter"},
                                                                                        {"action", "index"},
                                                                                        {"id", ""}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Layouts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }, 
                            new RouteDescriptor {
                                                     Priority = 1,
                                                     Route = new Route(
                                                         "Projector/sortcriterion/{action}/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Layouts"},
                                                                                      {"controller", "SortCriteria"},
                                                                                        {"action", "index"},
                                                                                        {"id", ""}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Layouts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }

  
                         };
        }
    }
}