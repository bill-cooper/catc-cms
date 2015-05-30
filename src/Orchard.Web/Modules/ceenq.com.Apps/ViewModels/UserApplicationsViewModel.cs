using System.Collections.Generic;
using ceenq.com.Core.Models;
using Orchard.Security;

namespace ceenq.com.Apps.ViewModels {
    public class UserApplicationsViewModel {
        public UserApplicationsViewModel() {
            Applications = new List<UserApplicationEntry>();
        }

        public IUser User { get; set; }
        public IUserApplications UserApplications { get; set; }
        public IList<UserApplicationEntry> Applications { get; set; }
    }

    public class UserApplicationEntry
    {
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public bool UserHasApplicationAccess { get; set; }
    }
}