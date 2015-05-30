using ceenq.com.Core.Applications;
using Orchard;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServerConfigService: IDependency
    {
        string GenerateConfig(IApplication application);
        IRoutingConfigFile BuildConfig(IApplication application);
    }

    public class DefaultRoutingServerConfigService : IRoutingServerConfigService
    {
        public string GenerateConfig(IApplication application)
        {
            throw new System.NotImplementedException();
        }

        public IRoutingConfigFile BuildConfig(IApplication application)
        {
            throw new System.NotImplementedException();
        }
    }
}