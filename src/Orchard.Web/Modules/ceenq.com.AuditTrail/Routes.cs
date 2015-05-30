using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ceenq.com.AuditTrail
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var route in GetRoutes()) routes.Add(route);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            yield return new RouteDescriptor
            {
                Route = new Route(
                    "Admin/AuditTrail/{controller}/{action}/{id}",
                    new RouteValueDictionary {
                        {"area", "Orchard.AuditTrail"},
                        {"id", UrlParameter.Optional}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary() {
                        {"area", "Orchard.AuditTrail"}
                    },
                    new MvcRouteHandler())
            };
        }
    }
}