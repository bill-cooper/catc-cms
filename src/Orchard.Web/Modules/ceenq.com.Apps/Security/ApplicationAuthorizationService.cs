using System;
using System.Runtime.Serialization;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using ceenq.com.Core.Security;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;

namespace ceenq.com.Apps.Security
{
    [UsedImplicitly]
    public class ApplicationAuthorizationService : Component, IApplicationAuthorizationService
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IApplicationAuthorizationServiceEventHandler _authorizationServiceEventHandler;

        public ApplicationAuthorizationService(IWorkContextAccessor workContextAccessor, IApplicationAuthorizationServiceEventHandler authorizationServiceEventHandler)
        {
            _workContextAccessor = workContextAccessor;
            _authorizationServiceEventHandler = authorizationServiceEventHandler;
        }

        public void CheckAccess(IUser user, IApplication application)
        {
            if (!TryCheckAccess(user, application))
            {
                throw new ApplicationSecurityException(T("An account security exception occurred."))
                {
                    ApplicationName = application.Name,
                    User = user
                };
            }
        }

        public bool TryCheckAccess(IUser user, IApplication application)
        {
            var context = new CheckApplicationAccessContext { User = user, Application = application };
            _authorizationServiceEventHandler.Checking(context);

            if (!context.Granted && context.User != null)
            {
                if (!String.IsNullOrEmpty(_workContextAccessor.GetContext().CurrentSite.SuperUser) &&
                       String.Equals(context.User.UserName, _workContextAccessor.GetContext().CurrentSite.SuperUser, StringComparison.Ordinal))
                {
                    context.Granted = true;
                }
            }

            _authorizationServiceEventHandler.Complete(context);
            return context.Granted;
        }
    }

    public class ApplicationSecurityException : OrchardCoreException
    {
        public ApplicationSecurityException(LocalizedString message) : base(message) { }
        public ApplicationSecurityException(LocalizedString message, Exception innerException) : base(message, innerException) { }
        protected ApplicationSecurityException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public string ApplicationName { get; set; }
        public IUser User { get; set; }
        public IContent Content { get; set; }
    }
}
