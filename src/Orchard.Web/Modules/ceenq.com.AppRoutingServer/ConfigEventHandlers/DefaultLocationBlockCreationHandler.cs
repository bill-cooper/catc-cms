using System;
using System.Linq;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class DefaultLocationBlockCreationHandler : ILocationBlockCreationHandler
    {
        private readonly ILocationBlockConfigurationHandler _locationBlockConfigurationHandler;
        private readonly ILocationBlockAdjustHandler _locationBlockAdjustHandler;
        private readonly ILocationBlockFinalizeHandler _locationBlockFinalizeHandler;
        public Localizer T { get; set; }
        public DefaultLocationBlockCreationHandler(ILocationBlockConfigurationHandler locationBlockConfigurationHandler, ILocationBlockAdjustHandler locationBlockAdjustHandler, ILocationBlockFinalizeHandler locationBlockFinalizeHandler)
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

            //if no base directory route has been defined for the application, then insert a default route
            // to handle requests to the base directory
            // This default route will route traffic to the root asset directory for the account

            //if a base directory route exists, then return
            if (context.Application.Routes.Any(r => r.RequestPattern == "/")) return;
            //if an exact base directory route exists, then return
            if (context.Application.Routes.Any(r => r.RequestPattern == "=/")) return;

            var route = new DefaultRouteImpl
            {
                RequestPattern = "/",
                PassTo = "/"
            };

            var locationBlock = new LocationBlock
            {
                MatchPattern = route.RequestPattern,
                Order = Int32.MaxValue //make default location least priority of all routes
            };


            var locationBlockContext = new LocationBlockContext(locationBlock,context.Application, route, context.AccountContext);
            _locationBlockConfigurationHandler.ConfigureLocationBlock(locationBlockContext);
            _locationBlockAdjustHandler.AdjustLocationBlock(locationBlockContext);
            _locationBlockFinalizeHandler.FinalizeLocationBlock(locationBlockContext);

            context.ServerBlock.LocationBlocks.Add(locationBlock);

        }
    }
}