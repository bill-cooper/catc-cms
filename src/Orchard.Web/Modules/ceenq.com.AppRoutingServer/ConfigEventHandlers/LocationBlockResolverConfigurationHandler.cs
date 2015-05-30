using System;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockResolverConfigurationHandler : ILocationBlockAdjustHandler
    {
        private readonly IRouteService _routeService;
        public Localizer T { get; set; }
        public LocationBlockResolverConfigurationHandler(IRouteService routeService)
        {
            _routeService = routeService;
            T = NullLocalizer.Instance;
        }

        public void AdjustLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not adjust location block.  The config context was not supplied."), new ArgumentException("Could not adjust location block.  The config context was not supplied.", "context"));

            if (!_routeService.RoutesToLocalResource(context.Route))
            {
                //default google resolver
                context.LocationBlock.Resolver = "8.8.8.8";
            }
        }
    }
}