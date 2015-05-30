using System.Collections;
using System.Collections.Generic;
using ceenq.com.Core.Routing;
using Orchard.ContentManagement;

namespace ceenq.com.Core.Applications
{
    public interface IApplication : IContent
    {
        int Id { get; }
        string Name { get; set; }
        string AuthenticationRedirect { get; set; }
        string ResetPassword { get; set; }
        string AccountVerification { get; set; }
        string Domain { get; set; }
        string IpAddress { get; }
        bool SuppressDefaultEndpoint { get; set; }

        IList<IRoute> Routes { get; set;  }
        bool TransportSecurity { get; set; }
    }
}
