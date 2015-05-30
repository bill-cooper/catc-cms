using Orchard;
using Orchard.Recipes.Models;
using Orchard.Security;

namespace ceenq.com.Core.Tenants
{
    public interface ITenantRecipe : IDependency
    {
        Recipe Build(string name);
    }

    public class DefaultTenantRecipe : DefaultImplementationNotifier, ITenantRecipe
    {
        public DefaultTenantRecipe(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public Recipe Build(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}