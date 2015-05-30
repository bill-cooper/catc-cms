using System;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IServerCommandClient
    {
        ServerCommandResult ExecuteCommand(IServerCommand command);
    }
    public class DefaultServerCommandClient : DefaultImplementationNotifier, IServerCommandClient
    {
        public DefaultServerCommandClient(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public ServerCommandResult ExecuteCommand(IServerCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
