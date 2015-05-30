using System;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockMaxBodySizeConfigurationHandler : ILocationBlockConfigurationHandler
    {
        public Localizer T { get; set; }
        public LocationBlockMaxBodySizeConfigurationHandler()
        {
            T = NullLocalizer.Instance;
        }
        public void ConfigureLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not configure location block.  The config context was not supplied."), new ArgumentException("Could not configure location block.  The config context was not supplied.", "context"));

            context.LocationBlock.ClientMaxBodySize = "64M";
        }
    }
}