using System.Collections.Generic;
using ceenq.com.Apps.Models;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Apps.Services
{
    public interface IApplicationManager : IDependency
    {
        IEnumerable<IApplication> GetApplications(bool includeDynamicApplications = false);

        IEnumerable<ApplicationPart> GetApplicationParts();
        void UpdateUsersApplications(IUser user, List<string> applications);
        void AddUserToApplication(IUser user, string application);
        void RemoveUserFromApplication(IUser user, string application);
        ApplicationPart GetApplication(int id);
        IApplication GetApplicationByName(string application);
        ApplicationPart CreateApplication(ApplicationPart application);
        void DeleteApplication(int id);
        void UpdateApplication(ApplicationPart application);
        RoutePart NewRoute(ApplicationPart application);
        RoutePart GetRoute(int id);
        IList<RoutePart> GetRoutes(int applicationId);
    }
}