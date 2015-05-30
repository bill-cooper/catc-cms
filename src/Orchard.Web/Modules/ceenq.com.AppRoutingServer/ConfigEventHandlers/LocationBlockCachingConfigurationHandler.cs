using System;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockCachingConfigurationHandler : ILocationBlockConfigurationHandler
    {
        public Localizer T { get; set; }
        public LocationBlockCachingConfigurationHandler()
        {
            T = NullLocalizer.Instance;
        }
        public void ConfigureLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not configure location block.  The config context was not supplied."), new ArgumentException("Could not configure location block.  The config context was not supplied.", "context"));

             if (!context.Route.CachingEnabled) return;

             context.LocationBlock.ProxyCache = "cache";
             context.LocationBlock.Header.Add("Cache-Control \"public\"");
             context.LocationBlock.Header.Add("X-Cache $upstream_cache_status");
             context.LocationBlock.Expires = "1h";
             context.LocationBlock.ProxyIgnoreHeaders.Add("Set-Cookie");
             context.LocationBlock.ProxyIgnoreHeaders.Add("Cache-Control");
        }
    }
}