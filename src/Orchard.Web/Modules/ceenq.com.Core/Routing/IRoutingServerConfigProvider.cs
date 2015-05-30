using System;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using Orchard;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerConfigManager : IDependency
    {
        void SaveConfig(IApplication application, IRoutingServer routingServer);
        void SaveConfig(IApplication application, IRoutingServer routingServer, string configFile);
        void DeleteConfig(IApplication application, IRoutingServer routingServer);
    }

    public class DefaultRoutingServerConfigManager : IRoutingServerConfigManager
    {
        public void SaveConfig(IApplication application, IRoutingServer routingServer)
        {
            throw new NotImplementedException();
        }

        public void SaveConfig(IApplication application, IRoutingServer routingServer, string configFile)
        {
            throw new NotImplementedException();
        }

        public void DeleteConfig(IApplication application, IRoutingServer routingServer)
        {
            throw new NotImplementedException();
        }
    }
}
