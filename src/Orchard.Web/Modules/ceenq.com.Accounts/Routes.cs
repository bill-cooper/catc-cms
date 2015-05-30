using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.Accounts
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
                            //The following route provides the account controller index in place of the admin dashboard screen.
                             new RouteDescriptor {
                                                     Priority = 1,
                                                     Route = new Route(
                                                         "Admin",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "admin"},
                                                                                      {"action", "index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/Log",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "Log"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/Log/Clear",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "Clear"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/Log/Refresh",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "Refresh"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/RestartWebServer",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "RestartWebServer"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/Restart",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "RestartVm"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/PowerOn",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "PowerOnVm"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/PowerOff",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "PowerOffVm"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/RoutingServers/{ipAddress}/Delete",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "RoutingServer"},
                                                                                      {"action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/Applications",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "Application"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/Applications/Create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "Application"},
                                                                                      {"action", "Create"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/Applications/{applicationName}/Config",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "Application"},
                                                                                      {"action", "ViewConfig"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountName}/Applications/{applicationName}/Log",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "Application"},
                                                                                      {"action", "ViewLog"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Admin/Account/{accountId}/Applications/{applicationId}/Edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "Application"},
                                                                                      {"action", "Edit"}
                                                                                  },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                              //The following route overrides orchard's default catchall route in order to direct all non-admin requests to the RedirectController
                              // which will redirect to the admin
                              new RouteDescriptor {                                                     
                                                     Priority = -10,
                                                     Route = new Route(
                                                         "{controller}/{action}/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"},
                                                                                      {"controller", "Redirect"},
                                                                                      {"action", "index"},
                                                                                      {"id", ""},
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"controller", new IsRedirect()}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "ceenq.com.Accounts"}
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