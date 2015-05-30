using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.Apps
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
                              //The following route overrides orchard's default catchall route in order to direct all non-admin requests to the RedirectController
                              // which will redirect to the admin
                              new RouteDescriptor {                                                     
                                                     Priority = -10,
                                                     Route = new Route(
                                                         "{controller}/{action}/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Apps"},
                                                                                      {"controller", "Redirect"},
                                                                                      {"action", "index"},
                                                                                      {"id", ""},
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"controller", new IsRedirect()}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Apps"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
        public class IsRedirect : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                object value;
                if (values.TryGetValue(parameterName, out value))
                {
                    var parameterValue = Convert.ToString(value);
                    return string.Equals(parameterValue, "redirect", StringComparison.OrdinalIgnoreCase);
                }
                return false;
            }
        }
    }
}