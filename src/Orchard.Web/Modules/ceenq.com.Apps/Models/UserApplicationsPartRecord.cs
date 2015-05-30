using System.Collections.Generic;
using Orchard.ContentManagement.Records;

namespace ceenq.com.Apps.Models {
    public class UserApplicationsPartRecord : ContentPartRecord
    {        
        public UserApplicationsPartRecord()
        {
            Applications = new List<UserApplicationJoinRecord>();
        }
        public virtual IList<UserApplicationJoinRecord> Applications { get; set; }
    }
    public class UserApplicationJoinRecord
    {
        public virtual int Id { get; set; }
        public virtual ApplicationRecord ApplicationRecord { get; set; }
        public virtual UserApplicationsPartRecord UserApplicationsPartRecord { get; set; }
    }
}