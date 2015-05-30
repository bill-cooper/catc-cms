using System;
using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Environment.Extensions;

namespace ceenq.com.AppRoutingServer.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Routing.DefaultRoutingServerConfigService")]
    public class NginxConfigService : IRoutingServerConfigService
    {
        private readonly IServerBlockCreationHandler _serverBlockCreationHandlers;
        private readonly IServerBlockAdjustHandler _serverBlockAdjustHandlers;
        private readonly INginxConfigSerializer _nginxConfigSerializer;
        public NginxConfigService(IServerBlockCreationHandler serverBlockCreationHandlers, IServerBlockAdjustHandler serverBlockAdjustHandlers, INginxConfigSerializer nginxConfigSerializer)
        {
            _serverBlockCreationHandlers = serverBlockCreationHandlers;
            _serverBlockAdjustHandlers = serverBlockAdjustHandlers;
            _nginxConfigSerializer = nginxConfigSerializer;
        }

        public string GenerateConfig(IApplication application)
        {
            return _nginxConfigSerializer.Serialize(BuildConfig(application));
        }

        public Config BuildConfig(IApplication application)
        {
            var configContext = new ConfigContext(new Config(), application);

            _serverBlockCreationHandlers.AddServerBlock(configContext);
            _serverBlockAdjustHandlers.AdjustServerBlock(configContext);

            return configContext.Config;
        }
        IRoutingConfigFile IRoutingServerConfigService.BuildConfig(IApplication application)
        {
            return BuildConfig(application);
        }
    }
}