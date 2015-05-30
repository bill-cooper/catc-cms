using System;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockHostHeaderConfigurationHandler : ILocationBlockConfigurationHandler
    {
        private readonly IRouteService _routeService;
        public Localizer T { get; set; }
        public LocationBlockHostHeaderConfigurationHandler(IRouteService routeService)
        {
            _routeService = routeService;
            T = NullLocalizer.Instance;
        }

        public void ConfigureLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not configure location block.  The config context was not supplied."), new ArgumentException("Could not configure location block.  The config context was not supplied.", "context"));


            if (!_routeService.RoutesToLocalResource(context.Route))
            {
                context.LocationBlock.ProxySetHeader.Add("App-Host $http_host");
            }
        }
    }
}