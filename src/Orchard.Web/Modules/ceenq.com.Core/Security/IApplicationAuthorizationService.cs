using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Security
{
    public interface IApplicationAuthorizationService : IDependency
    {
        void CheckAccess(IUser user, IApplication application);
        bool TryCheckAccess(IUser user, IApplication application);
    }
}
