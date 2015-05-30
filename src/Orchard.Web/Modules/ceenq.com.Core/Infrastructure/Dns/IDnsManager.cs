using System;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Infrastructure.Dns
{
    public interface IDnsManager: IDependency
    {
        void CreateARecord(string host, string ipAddress);
        void CreateARecord(string host, string ipAddress, string accessKey, string secretAccessKey, string hostedZone);
        void CreateCName(string host, string toAddress, string accessKey, string secretAccessKey, string hostedZone);
    }

    public class DefaultDnsManager :DefaultImplementationNotifier, IDnsManager
    {
        public DefaultDnsManager(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public void CreateARecord(string host, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public void CreateARecord(string host, string ipAddress, string accessKey, string secretAccessKey, string hostedZone)
        {
            throw new NotImplementedException();
        }

        public void CreateCName(string host, string toAddress, string accessKey, string secretAccessKey, string hostedZone)
        {
            throw new NotImplementedException();
        }
    }
}