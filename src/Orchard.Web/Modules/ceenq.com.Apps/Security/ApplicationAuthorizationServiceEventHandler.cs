using System;
using System.Linq;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using ceenq.com.Core.Security;
using Orchard;
using Orchard.ContentManagement;

namespace ceenq.com.Apps.Security
{
    public class ApplicationAuthorizationServiceEventHandler : IApplicationAuthorizationServiceEventHandler
    {
        private readonly IApplicationRequestContext _applicationContext;

        public ApplicationAuthorizationServiceEventHandler(IApplicationRequestContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Checking(CheckApplicationAccessContext context) { }
        public void Adjust(CheckApplicationAccessContext context) { }

        public void Complete(CheckApplicationAccessContext context)
        {
            //Temp comment this out
            //context.Granted = false;
            ////grant application authorization if the user has been assoicated with this application
            //var userApplications = context.User.As<IUserApplications>();
            //if (
            //    userApplications.ApplicationNames.Any(
            //        appName =>
            //            String.Equals(appName, _applicationContext.Application.Name,
            //                StringComparison.InvariantCultureIgnoreCase)))
            //{
                context.Granted = true;
            //}
        }
    }
}