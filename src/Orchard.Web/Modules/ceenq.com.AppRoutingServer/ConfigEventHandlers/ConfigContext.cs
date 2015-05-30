using ceenq.com.Core.Applications;
using ceenq.com.RoutingServer.Configuration;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class ConfigContext
    {
        public ConfigContext(Config config, IApplication application)
        {
            Config = config;
            Application = application;
        }
        public Config Config { get; private set; }
        public IApplication Application { get; private set; }
    }
}