using System.Collections.Generic;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace ceenq.com.ApplicationAPI
{
    public class ApiRoutes : IHttpRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (RouteDescriptor routeDescriptor in GetRoutes()) routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                 new HttpRouteDescriptor {
                                 Name = "AuthApiControllerAndAction",
                                 RouteTemplate = "cnq/api/auth/{action}",
                                 Defaults = new {
                                                    area = "ceenq.com.ApplicationAPI",
                                                    controller = "AuthApi"
                                                 }
                                 }
            };
        }
    }
}