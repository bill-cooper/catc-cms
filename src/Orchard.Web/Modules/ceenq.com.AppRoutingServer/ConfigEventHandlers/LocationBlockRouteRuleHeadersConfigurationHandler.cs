using System;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockRouteRuleHeadersConfigurationHandler : ILocationBlockConfigurationHandler
    {
        public Localizer T { get; set; }
        public LocationBlockRouteRuleHeadersConfigurationHandler()
        {
            T = NullLocalizer.Instance;
        }
        public void ConfigureLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not configure location block.  The config context was not supplied."), new ArgumentException("Could not configure location block.  The config context was not supplied.", "context"));

            if (string.IsNullOrWhiteSpace(context.Route.Rules)) return;

            var rules = context.Route.Rules.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var rule in rules)
            {
                var ruleParts = rule.Trim().Split(new[] { '=' });
                if (ruleParts.Length != 2) continue;
                var ruleKey = ruleParts[0].Trim().Replace(" ", "_");
                var ruleValue = ruleParts[1].Trim().Replace(" ", "_");
                context.LocationBlock.ProxySetHeader.Add(string.Format("{0} {1}", ruleKey.ToLower(), ruleValue.ToLower()));
            }

        }
    }
}