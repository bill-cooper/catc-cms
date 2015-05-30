using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.RoutingServer.Configuration;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class ServerBlockContext
    {
        public ServerBlockContext(ServerBlock serverBlock, IApplication application, IAccountContext accountContext)
        {
            ServerBlock = serverBlock;
            Application = application;
            AccountContext = accountContext;
        }
        public ServerBlock ServerBlock { get; private set; }
        public IApplication Application { get; private set; }
        public IAccountContext AccountContext { get; private set; }
    }
}