using System.Collections.Generic;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using Orchard.ContentManagement;

namespace ceenq.com.DashboardApp
{
    public class ApplicationImpl : IApplication
    {
        public ApplicationImpl()
        {
            Routes = new List<IRoute>();
        }
        public ContentItem ContentItem { get; private set; }
        public int Id { get; private set; }
        public string Name { get; set; }
        public string AuthenticationRedirect { get; set; }
        public string ResetPassword { get; set; }
        public string AccountVerification { get; set; }
        public string Domain { get; set; }
        public string IpAddress { get; set; }
        public bool SuppressDefaultEndpoint { get; set; }
        public IList<IRoute> Routes { get; set; }
        public bool TransportSecurity { get; set; }
    }
}