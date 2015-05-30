using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Models;
using Orchard.ContentManagement;

namespace ceenq.com.Apps.Models {
    public class UserApplicationsPart : ContentPart<UserApplicationsPartRecord> , IUserApplications {

        public IEnumerable<ApplicationRecord> Applications 
        {
            get
            {
                return Record.Applications.Select(r => r.ApplicationRecord);
            }
        }
        public IEnumerable<string> ApplicationNames {
            get { return Applications.Select(a => a.Name); }
        }

    }
}