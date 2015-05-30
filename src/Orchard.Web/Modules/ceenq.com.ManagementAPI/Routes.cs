using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace ceenq.com.ManagementAPI
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
                                Name = "ApplicationManagementApiControllerAndAction",
                                RouteTemplate = "cnq/api/apps/{id}",
                                Defaults = new {
                                                   area = "ceenq.com.ManagementAPI",
                                                   controller = "ApplicationManagementApi",
                                                   id = RouteParameter.Optional
                                                }
                                },
                new HttpRouteDescriptor {
                                Name = "AssetManagementApiControllerAndAction",
                                RouteTemplate = "cnq/api/documents/{id}",
                                Defaults = new {
                                                   area = "ceenq.com.ManagementAPI",
                                                   controller = "AssetManagementApi",
                                                   id = RouteParameter.Optional
                                                }
                                },
                new HttpRouteDescriptor {
                                Name = "IpAddressManagementApiControllerAndAction",
                                RouteTemplate = "cnq/api/endpoints/{id}",
                                Defaults = new {
                                                   area = "ceenq.com.ManagementAPI",
                                                   controller = "IpAddressManagementApi",
                                                   id = RouteParameter.Optional
                                                }
                                },
    
                new HttpRouteDescriptor {
                                 Name = "DirectoryApiGet",
                                 RouteTemplate = "cnq/api/directory/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    action = "Get",
                                                    controller = "DirectoryApi",
                                                    id = RouteParameter.Optional
                                                 },
                                Constraints = new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)}
                                 },
                new HttpRouteDescriptor {
                                 Name = "DirectoryApiPost",
                                 RouteTemplate = "cnq/api/directory/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    action = "Post",
                                                    controller = "DirectoryApi",
                                                    id = RouteParameter.Optional
                                                 },
                                Constraints = new {httpMethod = new HttpMethodConstraint(HttpMethod.Post)}
                                 },
                new HttpRouteDescriptor {
                                 Name = "DirectoryApiPut",
                                 RouteTemplate = "cnq/api/directory/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    action = "Put",
                                                    controller = "DirectoryApi",
                                                    id = RouteParameter.Optional
                                                 },
                                Constraints = new {httpMethod = new HttpMethodConstraint(HttpMethod.Put)}
                                 },
                new HttpRouteDescriptor {
                                 Name = "DirectoryApiDelete",
                                 RouteTemplate = "cnq/api/directory/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    action = "Delete",
                                                    controller = "DirectoryApi"
                                                 },
                                Constraints = new {httpMethod = new HttpMethodConstraint(HttpMethod.Delete)}
                                 },
                new HttpRouteDescriptor {
                                 Name = "DirectoryApiWithAction",
                                 RouteTemplate = "cnq/api/directory/{id}/{action}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    controller = "DirectoryApi",
                                                    id = RouteParameter.Optional
                                                 }
                                 },
                new HttpRouteDescriptor {
                                 Name = "FilesApiGet",
                                 RouteTemplate = "cnq/api/files/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    action = "Get",
                                                    controller = "FilesApi"
                                                 },
                                Constraints = new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)}
                                 },
                new HttpRouteDescriptor {
                                 Name = "FilesApiWithAction",
                                 RouteTemplate = "cnq/api/files/{action}/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    controller = "FilesApi",
                                                    id = RouteParameter.Optional
                                                 }
                                 },
                
                new HttpRouteDescriptor {
                                 Name = "FilesApiPost",
                                 RouteTemplate = "cnq/api/files",
                                 Defaults = new {
                                                    area = "ceenq.com.ManagementAPI",
                                                    action = "Post",
                                                    controller = "FilesApi"
                                                 },
                                Constraints = new {httpMethod = new HttpMethodConstraint(HttpMethod.Post)}
                                 },
                new HttpRouteDescriptor {
                                 Name = "UserControllerAndAction",
                                 RouteTemplate = "cnq/api/users/{id}",
                                 Defaults = new {
                                                    area = "ceenq.com.Users",
                                                    controller = "UserApi",
                                                    id = RouteParameter.Optional
                
                                                 }
                                 }
            };
        }
    }
}