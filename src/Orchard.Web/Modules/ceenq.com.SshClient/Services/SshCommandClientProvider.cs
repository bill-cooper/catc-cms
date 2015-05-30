using ceenq.com.Core.Infrastructure.Compute;
using Orchard.Environment.Extensions;

namespace ceenq.com.SshClient.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Infrastructure.Compute.DefaultServerCommandClientProvider")]
    public class SshCommandClientProvider : IServerCommandClientProvider
    {
        private readonly IServerCommandEventHandler _serverCommandEventHandler;
        public SshCommandClientProvider(IServerCommandEventHandler serverCommandEventHandler)
        {
            _serverCommandEventHandler = serverCommandEventHandler;
        }

        public IServerCommandClient Connect(IServerCommandClientContext context)
        {
            return new SshCommandClient((SshConnectionInfo)context,_serverCommandEventHandler);
        }
    }
}