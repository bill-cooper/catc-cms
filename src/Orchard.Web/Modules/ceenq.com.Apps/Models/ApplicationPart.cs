using System.Collections.Generic;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using Orchard.ContentManagement;

namespace ceenq.com.Apps.Models
{
    public class ApplicationPart: ContentPart<ApplicationRecord> , IApplication
    {
        public ApplicationPart()
        {
            Routes = new List<IRoute>();
        }
        int IApplication.Id {
            get { return Id; }
        }

        public string Name
        {
            get { return Retrieve(x => x.Name); }
            set { Store(x => x.Name, value); }
        }
        public string AuthenticationRedirect
        {
            get { return Retrieve(x => x.AuthenticationRedirect); }
            set { Store(x => x.AuthenticationRedirect, value); }
        }
        public string AccountVerification
        {
            get { return Retrieve(x => x.AccountVerification); }
            set { Store(x => x.AccountVerification, value); }
        }
        public string ResetPassword
        {
            get { return Retrieve(x => x.ResetPassword); }
            set { Store(x => x.ResetPassword, value); }
        }
        public string Domain
        {
            get { return Retrieve(x => x.Domain); }
            set { Store(x => x.Domain, value); }
        }

        public string IpAddress {
            get
            {
                var appRoutingServer = this.As<IApplicationRoutingServer>();
                return appRoutingServer == null ? "" : appRoutingServer.IpAddress;
            }
        }

        public bool SuppressDefaultEndpoint
        {
            get { return Retrieve(x => x.SuppressDefaultEndpoint); }
            set { Store(x => x.SuppressDefaultEndpoint, value); }
        }

        public bool TransportSecurity
        {
            get { return Retrieve(x => x.TransportSecurity); }
            set { Store(x => x.TransportSecurity, value); }
        }
        public IList<IRoute> Routes { get; set; }

    }
}