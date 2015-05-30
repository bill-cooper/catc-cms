using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IServerCommandClientProvider: ISingletonDependency
    {
        IServerCommandClient Connect(IServerCommandClientContext context);
    }

    public class DefaultServerCommandClientProvider : DefaultImplementationNotifier, IServerCommandClientProvider
    {
        public DefaultServerCommandClientProvider(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public IServerCommandClient Connect(IServerCommandClientContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
