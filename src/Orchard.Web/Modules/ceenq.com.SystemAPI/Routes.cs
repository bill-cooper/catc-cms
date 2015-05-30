using System.Collections.Generic;
using System.Web.Http;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace ceenq.com.SystemAPI
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
                                 Name = "AccountControllerAndAction",
                                 RouteTemplate = "cnq/api/account/{action}/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.Accounts",
                                                    controller = "AccountApi",
                                                    id = RouteParameter.Optional
                                                 }
                                 }
            };
        }
    }
}