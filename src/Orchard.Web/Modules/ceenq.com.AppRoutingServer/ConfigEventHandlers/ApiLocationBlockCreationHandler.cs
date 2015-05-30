using System;
using System.Linq;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class ApiLocationBlockCreationHandler: ILocationBlockCreationHandler
    {
        private readonly ILocationBlockConfigurationHandler _locationBlockConfigurationHandler;
        private readonly ILocationBlockAdjustHandler _locationBlockAdjustHandler;
        private readonly ILocationBlockFinalizeHandler _locationBlockFinalizeHandler;
        public Localizer T { get; set; }
        public ApiLocationBlockCreationHandler(ILocationBlockConfigurationHandler locationBlockConfigurationHandler, ILocationBlockAdjustHandler locationBlockAdjustHandler, ILocationBlockFinalizeHandler locationBlockFinalizeHandler)
        {
            _locationBlockConfigurationHandler = locationBlockConfigurationHandler;
            _locationBlockAdjustHandler = locationBlockAdjustHandler;
            _locationBlockFinalizeHandler = locationBlockFinalizeHandler;
            T = NullLocalizer.Instance;
        }

        public void AddLocationBlock(ServerBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate location block.  The config context was not supplied."), new ArgumentException("Could not generate location block.  The config context was not supplied.", "context"));

            //if no api route has been defined for the application, then insert a default location
            // to handle the api request pattern

            //if a base api exists, then return
            if (context.Application.Routes != null && context.Application.Routes.Any(r => r.RequestPattern == "~* ^/api/(.*)$")) return;

            var route = new DefaultRouteImpl
            {
                RequestPattern = "~* ^/api/(.*)$",
                PassTo = context.AccountContext.InternalAbsoluteApiPath,
                RouteOrder = 0
            };

            var locationBlock = new LocationBlock
            {
                MatchPattern = route.RequestPattern,
                Order = route.RouteOrder
            };


            var locationBlockContext = new LocationBlockContext(locationBlock, context.Application, route, context.AccountContext);
            _locationBlockConfigurationHandler.ConfigureLocationBlock(locationBlockContext);
            _locationBlockAdjustHandler.AdjustLocationBlock(locationBlockContext);
            _locationBlockFinalizeHandler.FinalizeLocationBlock(locationBlockContext);

            context.ServerBlock.LocationBlocks.Add(locationBlock);
        }
    }
}