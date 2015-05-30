using System;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Utility;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockAdjustMatchPatternHandler : ILocationBlockAdjustHandler
    {
        public Localizer T { get; set; }
        public LocationBlockAdjustMatchPatternHandler()
        {
            T = NullLocalizer.Instance;
        }

        public void AdjustLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not adjust location block.  The config context was not supplied."), new ArgumentException("Could not adjust location block.  The config context was not supplied.", "context"));

            //take no action if this is a named location
            if (context.LocationBlock.MatchPattern.StartsWith("@")) return;


            if (!context.LocationBlock.MatchPattern.StartsWith("~*") //add the case-insensitive regex modifier if it does not alread exist
                &&
                !context.LocationBlock.MatchPattern.StartsWith("=")  // and if the pattern is not referring to an exact match
               )
                context.LocationBlock.MatchPattern = string.Format("~* {0}", context.LocationBlock.MatchPattern);
        }
    }
}