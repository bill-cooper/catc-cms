using System;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Assets
{
    public interface IAssetStorageManager: IDependency {
        bool StorageExists(string name);
        void CreateStorage(string name);
        void DeleteStorage(string name);
    }

    public class DefaultAssetStorageManager : DefaultImplementationNotifier, IAssetStorageManager
    {
        public DefaultAssetStorageManager(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public bool StorageExists(string name) {
            throw new NotImplementedException();
        }

        public void CreateStorage(string name) {
            throw new NotImplementedException();
        }

        public void DeleteStorage(string name) {
            throw new NotImplementedException();
        }
    }
}
