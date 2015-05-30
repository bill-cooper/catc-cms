using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Security;

namespace ceenq.com.Core.Tenants
{
    public interface ITenantService : IDependency
    {
        ShellSettings Prime();
        string Setup(TenantCreationContext context);
    }

    public class DefaultTenantService : DefaultImplementationNotifier, ITenantService
    {
        public DefaultTenantService(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public ShellSettings Prime()
        {
            throw new System.NotImplementedException();
        }

        public string Setup(TenantCreationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}