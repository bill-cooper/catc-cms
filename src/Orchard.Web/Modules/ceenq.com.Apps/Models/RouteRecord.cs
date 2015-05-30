using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using Orchard.ContentManagement.Records;

namespace ceenq.com.Apps.Models
{
    public class RouteRecord : ContentPartRecord
    {
        public virtual string RequestPattern { get; set; }
        public virtual string PassTo { get; set; }
        public virtual string Rules { get; set; }
        public virtual int RouteOrder { get; set; }
        public virtual bool RequireAuthentication { get; set; }
        public virtual bool CachingEnabled { get; set; }
        public virtual int ApplicationId { get; set; }
    }
}