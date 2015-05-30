using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using Orchard.Events;
using Orchard.Security;

namespace ceenq.com.Core.Security {
    public interface IApplicationAuthorizationServiceEventHandler : IEventHandler {
        void Checking(CheckApplicationAccessContext context);
        void Adjust(CheckApplicationAccessContext context);
        void Complete(CheckApplicationAccessContext context);
    }

    public class CheckApplicationAccessContext {
        public IUser User { get; set; }
        public IApplication Application { get; set; }
        
        // true if the permission has been granted to the user.
        public bool Granted { get; set; }
    }
}

