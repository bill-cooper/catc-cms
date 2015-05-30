using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.Core.Navigation;
using Orchard.Security;

namespace  ceenq.com.Theme.LayoutsAdmin.Security {
    [UsedImplicitly]
    public class AuthorizationEventHandler : IAuthorizationServiceEventHandler {
        private readonly IContentManager _contentManager;

        public AuthorizationEventHandler(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public void Checking(CheckAccessContext context) { }

        public void Complete(CheckAccessContext context)
        {
            //REFACTOR: Set up logic here so that authorization is denied
            // if anyone other than admin is trying to access the orchard 
            // backend without going through cnq dashboard
           // context.Granted = false;
        }

        public void Adjust(CheckAccessContext context) {
  
        }
    }
}