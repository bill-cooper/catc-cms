using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IServerCommandProvider : ISingletonDependency
    {
        T New<T>(params string[] parameters) where T : class, IServerCommand;
    }

    public class DefaultServerCommandProvider : DefaultImplementationNotifier, IServerCommandProvider
    {
        public DefaultServerCommandProvider(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public T New<T>(params string[] parameters) where T : class, IServerCommand
        {
            throw new System.NotImplementedException();
        }
    }
}
