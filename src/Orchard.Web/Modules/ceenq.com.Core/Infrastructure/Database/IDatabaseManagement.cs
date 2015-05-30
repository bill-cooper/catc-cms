using System;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Infrastructure.Database
{
    public interface IDatabaseManagement : IDependency {
        DatabaseInfo Create(DatabaseCreateParameters parameters);
    }

    public class DefaultDatabaseManagement : DefaultImplementationNotifier, IDatabaseManagement
    {
        public DefaultDatabaseManagement(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public DatabaseInfo Create(DatabaseCreateParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
