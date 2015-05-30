using System.Collections.Generic;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerManager: IDependency
    {
        IRoutingServer Get(string ipAddress);
        IRoutingServer GetDefault();
        IServerCommandClient GetCommandClient(string ipAddress);
        IServerCommandClient GetCommandClient(IRoutingServer routingServer);
        void Delete(int id);
        void Restart(int id);
        IEnumerable<IRoutingServer> List();
        IRoutingServer New();
        bool IsDefault(IRoutingServer routingServer);
        void PowerOn(int id);
        void PowerOff(int id);
    }

    public class DefaultRoutingServerManager : IRoutingServerManager
    {
        public IRoutingServer Get(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public IRoutingServer GetDefault()
        {
            throw new System.NotImplementedException();
        }

        public IServerCommandClient GetCommandClient(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public IServerCommandClient GetCommandClient(IRoutingServer routingServer)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Restart(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IRoutingServer> List()
        {
            throw new System.NotImplementedException();
        }

        public IRoutingServer New()
        {
            throw new System.NotImplementedException();
        }

        public bool IsDefault(IRoutingServer routingServer)
        {
            throw new System.NotImplementedException();
        }

        public void PowerOn(int id)
        {
            throw new System.NotImplementedException();
        }

        public void PowerOff(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}