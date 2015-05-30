using ceenq.com.Core.Infrastructure.Compute;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerProvider : IDependency
    {
        IServerCommandClient GetDefaultCommandClient();
    }

    public class DefaultRoutingServerProvider :  IRoutingServerProvider
    {

        public IServerCommandClient GetDefaultCommandClient()
        {
            throw new System.NotImplementedException();
        }
    }
}
