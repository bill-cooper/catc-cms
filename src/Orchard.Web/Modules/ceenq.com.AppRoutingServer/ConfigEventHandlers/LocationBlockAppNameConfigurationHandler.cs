using System;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockAppNameConfigurationHandler : ILocationBlockConfigurationHandler
    {
        private readonly IRouteService _routeService;
        public Localizer T { get; set; }
        public LocationBlockAppNameConfigurationHandler(IRouteService routeService)
        {
            _routeService = routeService;
            T = NullLocalizer.Instance;
        }

        public void ConfigureLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not configure location block.  The config context was not supplied."), new ArgumentException("Could not configure location block.  The config context was not supplied.", "context"));

            //if the request pattern refers to cnq system, then add the app name header
            //local resources can be excluded.  we only want to target request that are going
            //through the routing server and being proxy_pass'd on to the cnq system
            if (_routeService.RoutesToInternalResource(context.Route) && !_routeService.RoutesToLocalResource(context.Route))
            {
                context.LocationBlock.ProxySetHeader.Add(string.Format("App-Name {0}",context.Application.Name));
            }
        }
    }
}