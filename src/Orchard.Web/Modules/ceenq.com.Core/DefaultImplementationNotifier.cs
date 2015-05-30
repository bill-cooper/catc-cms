using System;
using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;

namespace ceenq.com.Core
{
    public class DefaultImplementationNotifier : INotificationProvider
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IAuthenticationService _authenticationService;
        public DefaultImplementationNotifier(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService)
        {
            _workContextAccessor = workContextAccessor;
            _authenticationService = authenticationService;
        }

        public Localizer T { get; set; }
        public IEnumerable<NotifyEntry> GetNotifications()
        {
            if (GetType().Name != "DefaultImplementationNotifier")
            {
                var user = _authenticationService.GetAuthenticatedUser();
                if (user != null && !String.IsNullOrEmpty(_workContextAccessor.GetContext().CurrentSite.SuperUser) &&
                          String.Equals(user.UserName, _workContextAccessor.GetContext().CurrentSite.SuperUser, StringComparison.Ordinal))
                {
                    var unimplementedInterface = this.GetType().GetInterfaces().FirstOrDefault(i => i.Name != "INotificationProvider" && i.Name != "IDependency");
                    yield return new NotifyEntry { Message = T(string.Format("You must enable a module that provides an implementation of {0}.", unimplementedInterface)), Type = NotifyType.Warning };

                }
            }
        }
    }
}