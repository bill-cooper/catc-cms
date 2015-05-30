using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ceenq.com.Accounts.ViewModels {
    public class ApplicationEditViewModel  {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public string AuthenticationRedirect { get; set; }
        public string ResetPassword { get; set; }
        public string AccountVerification { get; set; }
        public IList<RouteViewModel> Routes { get; set; }
        public string Domain { get; set; }
        public bool SuppressDefaultEndpoint { get; set; }
        public bool TransportSecurity { get; set; }
    }

    public class RouteViewModel
    {
        public int Id { get; set; }
        public string RequestPattern { get; set; }
        public string PassTo { get; set; }
        public int RouteOrder { get; set; }
        public bool RequireAuthentication { get; set; }
        public string Rules { get; set; }
        public bool CachingEnabled { get; set; }
    }

}
