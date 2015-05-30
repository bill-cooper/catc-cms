using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ceenq.com.ManagementAPI.Models
{
    public class Application
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string Url { get; set; }

        public string AuthenticationRedirect { get; set; }
        public string ResetPassword { get; set; }
        public string AccountVerification { get; set; }

        public IList<Route> Routes { get; set; }
        public string Domain { get; set; }
        public bool SuppressDefaultEndpoint { get; set; }
        public string IpAddress { get; set; }
        public bool TransportSecurity { get; set; }
    }

    public class Route {
        public int Id { get; set; }
        public string RequestPattern { get; set; }
        public string PassTo { get; set; }
        public string Rules { get; set; }
        public int RouteOrder { get; set; }
        public  bool RequireAuthentication { get; set; }
        public bool CachingEnabled { get; set; }
    }
}