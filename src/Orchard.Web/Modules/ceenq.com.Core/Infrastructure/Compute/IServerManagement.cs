using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IServerManagement: IDependency
    {
        ServerInfo Create(ServerOperationParameters parameters);
        void Delete(ServerOperationParameters parameters);
        void PowerOn(ServerOperationParameters parameters);
        void PowerOff(ServerOperationParameters parameters);
        bool IsPoweredOn(ServerOperationParameters parameters);
        void Reboot(ServerOperationParameters parameters);

    }

    public class DefaultServerManagement : DefaultImplementationNotifier, IServerManagement
    {
        public DefaultServerManagement(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public ServerInfo Create(ServerOperationParameters parameters)
        {
            throw new System.NotImplementedException();
        }
        public void Delete(ServerOperationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void PowerOn(ServerOperationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void PowerOff(ServerOperationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public bool IsPoweredOn(ServerOperationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void Reboot(ServerOperationParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
