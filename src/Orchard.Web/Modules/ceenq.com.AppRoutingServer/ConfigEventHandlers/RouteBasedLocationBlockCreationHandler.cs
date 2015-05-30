using System;
using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Utility;
using ceenq.com.RoutingServer.Configuration;
using NHibernate.Util;
using Orchard.Autoroute.Services;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class RouteBasedLocationBlockCreationHandler : ILocationBlockCreationHandler
    {
        private readonly ILocationBlockConfigurationHandler _locationBlockConfigurationHandler;
        private readonly ILocationBlockAdjustHandler _locationBlockAdjustHandler;
        private readonly ILocationBlockFinalizeHandler _locationBlockFinalizeHandler;
        private readonly ISlugService _slugService;
        private readonly IRouteService _routeService;
        public Localizer T { get; set; }
        public RouteBasedLocationBlockCreationHandler(ILocationBlockConfigurationHandler locationBlockConfigurationHandler, ILocationBlockAdjustHandler locationBlockAdjustHandler, ILocationBlockFinalizeHandler locationBlockFinalizeHandler, ISlugService slugService, IRouteService routeService)
        {
            _locationBlockConfigurationHandler = locationBlockConfigurationHandler;
            _slugService = slugService;
            _routeService = routeService;
            _locationBlockFinalizeHandler = locationBlockFinalizeHandler;
            _locationBlockAdjustHandler = locationBlockAdjustHandler;
            T = NullLocalizer.Instance;
        }

        public void AddLocationBlock(ServerBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate location block.  The config context was not supplied."), new ArgumentException("Could not generate location block.  The config context was not supplied.", "context"));

            //no routes, so return
            if (context.Application.Routes == null || !context.Application.Routes.Any()) return;


            var overloadedRequestPatterns = context.Application.Routes.GroupBy(r => r.RequestPattern)
                  .Select(rg => new
                  {
                      RequestPattern = rg.Key,
                      Count = rg.Count()
                  })
                  .Where(item => item.Count > 1);


            var linkedOverloadedRouteLookup = new Dictionary<string, LinkedList<IRoute>>();
            foreach (var overloadedRequestPattern in overloadedRequestPatterns)
            {
                var linkedRoutes = new LinkedList<IRoute>();
                //link then to each other and add them to another collection
                context.Application.Routes.Where(
                    r => r.RequestPattern == overloadedRequestPattern.RequestPattern)
                    .OrderBy(r => r.RouteOrder).ForEach(route => linkedRoutes.AddLast(route));
                linkedOverloadedRouteLookup.Add(overloadedRequestPattern.RequestPattern, linkedRoutes);
            }

            foreach (var route in context.Application.Routes)
            {
                var locationBlock = new LocationBlock { Order = route.RouteOrder };

                if (linkedOverloadedRouteLookup.ContainsKey(route.RequestPattern))
                {
                    var linkedRoutes = linkedOverloadedRouteLookup[route.RequestPattern];
                    var linkedOverloadedRoute = linkedRoutes.Find(route);

                    //then it's not the first one in the chain
                    if (linkedOverloadedRoute.Previous != null)
                    {
                        locationBlock.MatchPattern = string.Format("@{0}{1}", SafeLocationName(route.RequestPattern), linkedRoutes.ToList().IndexOf(route));
                    }

                    if (linkedOverloadedRoute.Next != null)//then it's not the last one in the chain
                    {
                        locationBlock.ProxyInterceptErrors = "on";
                        locationBlock.RecursiveErrorPages = "on";

                        var safeLocationName = string.Format("@{0}{1}", SafeLocationName(linkedOverloadedRoute.Next.Value.RequestPattern), linkedRoutes.ToList().IndexOf(linkedOverloadedRoute.Next.Value));

                        locationBlock.ErrorPage.Add("404 = " + safeLocationName);
                        locationBlock.ErrorPage.Add("403 = " + safeLocationName);
                    }
                }

                if (string.IsNullOrWhiteSpace(locationBlock.MatchPattern))
                {
                    locationBlock.MatchPattern = route.RequestPattern;

                    //if the request pattern to match starts with a slash, then we will consider this the
                    // beginning of a relative path that refers to the base directory.  In that case
                    // prepend a ^ which is the 'starts with' character for the regex
                    if (locationBlock.MatchPattern != "/" && locationBlock.MatchPattern.StartsWith("/"))
                        locationBlock.MatchPattern = string.Format("^{0}", locationBlock.MatchPattern);

                    //If the route is proxy passing and match pattern is a subdirectory, then make the match pattern a regex
                    // so that $1 can be used downstream in the config creation process to represent the requested path below
                    // the defined match pattern
                    if (!_routeService.RoutesToLocalResource(route)  //only for routes that target external resource.  Other than local asset store
                        && locationBlock.MatchPattern != "/"  //exclude the base route
                        && !locationBlock.MatchPattern.StartsWith("=")  //exclude exact match routes
                        && !locationBlock.MatchPattern.EndsWith("$")) //skip routes that already have a regex pattern set up
                    {
                        locationBlock.MatchPattern = string.Format("{0}(.*)$", locationBlock.MatchPattern);
                    }
                }

                var locationBlockContext = new LocationBlockContext(locationBlock, context.Application, route, context.AccountContext);
                _locationBlockConfigurationHandler.ConfigureLocationBlock(locationBlockContext);
                _locationBlockAdjustHandler.AdjustLocationBlock(locationBlockContext);
                _locationBlockFinalizeHandler.FinalizeLocationBlock(locationBlockContext);

                context.ServerBlock.LocationBlocks.Add(locationBlock);

            }
        }

        private string SafeLocationName(string path)
        {
            return _slugService.Slugify(path).Replace("/", "");
        }
    }
}