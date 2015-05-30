using System.Collections.Generic;
using System.Linq;
using ceenq.com.Apps.Models;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Data;
using Orchard.Security;

namespace ceenq.com.Apps.Services
{
    public class ApplicationManager : Component, IApplicationManager
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationEventHandler _applicationEvents;
        private readonly IDynamicApplicationEventHandler _dyanamicApplicationEventHandler;
        private readonly IRepository<UserApplicationJoinRecord> _userApplicationJoinRepository;

        public ApplicationManager(
            IOrchardServices orchardServices,
            IApplicationEventHandler applicationEvents, IRepository<UserApplicationJoinRecord> userApplicationJoinRepository, IDynamicApplicationEventHandler dyanApplicationEventHandler)
        {
            _orchardServices = orchardServices;
            _applicationEvents = applicationEvents;
            _userApplicationJoinRepository = userApplicationJoinRepository;
            _dyanamicApplicationEventHandler = dyanApplicationEventHandler;
        }

        public ApplicationPart GetApplication(int id)
        {
            var application = _orchardServices.ContentManager.Query<ApplicationPart, ApplicationRecord>().Where(u => u.Id == id).List().FirstOrDefault();
            if (application != null)
                application.Routes = GetRoutes(application.Id).OfType<IRoute>().ToList();
            return application;
        }
        public IApplication GetApplicationByName(string name)
        {
            var application = _orchardServices.ContentManager.Query<ApplicationPart, ApplicationRecord>().Where(u => u.Name == name).List().FirstOrDefault() as IApplication;
            if (application != null)
            {
                application.Routes = GetRoutes(application.Id).OfType<IRoute>().ToList();
            }
            else //if app was null, then try dynamic apps
            {
                var dynamicApplicationContext = new DynamicApplicationContext();
                _dyanamicApplicationEventHandler.FindDynamicApplications(dynamicApplicationContext);

                application = dynamicApplicationContext.Applications.FirstOrDefault(app => app.Name == name);
            }
            return application;
        }
        public IEnumerable<IApplication> GetApplications(bool includeDynamicApplications = false)
        {
            var applications =
                _orchardServices.ContentManager.Query<ApplicationPart, ApplicationRecord>()
                    .List()
                    .OfType<IApplication>()
                    .ToList();
            if (includeDynamicApplications)
            {
                var dynamicApplicationContext = new DynamicApplicationContext();
                _dyanamicApplicationEventHandler.FindDynamicApplications(dynamicApplicationContext);
                
                applications.AddRange(dynamicApplicationContext.Applications);
            }
            return applications;
        }

        public IEnumerable<ApplicationPart> GetApplicationParts()
        {
            return _orchardServices.ContentManager.Query<ApplicationPart, ApplicationRecord>().List();
        }

        public ApplicationPart CreateApplication(ApplicationPart application)
        {
            _applicationEvents.ApplicationCreating(new ApplicationEventContext() { Application = application });
            var existingApplication = GetApplicationByName(application.Name);
            if (existingApplication != null)
                throw new OrchardException(T("An application with name '{0}' already exists.  The application '{0}' cannot be created", application.Name));

            if (!ValidateApplication(application))
                throw new OrchardException(T("Invalid Application Name.  The application '{0}' cannot be created", application.Name));

            _orchardServices.ContentManager.Create(application.ContentItem, VersionOptions.Published);

            _applicationEvents.ApplicationCreated(new ApplicationEventContext() { Application = application });
            return application;
        }

        public RoutePart NewRoute(ApplicationPart application)
        {
            var route = _orchardServices.ContentManager.New<RoutePart>("Route");
            route.ApplicationId = application.Id;
            application.Routes.Add(route);
            return route;
        }
        private void CreateRoute(RoutePart routePart)
        {
            var application = GetApplication(routePart.ApplicationId);
            _applicationEvents.ApplicationUpdating(new ApplicationEventContext() { Application = application });
            routePart.As<ICommonPart>().Container = application.ContentItem;
            _orchardServices.ContentManager.Create(routePart);
        }
        private void UpdateRoute(RoutePart routePart)
        {
            var application = GetApplication(routePart.ApplicationId);
            _applicationEvents.ApplicationUpdating(new ApplicationEventContext() { Application = application });
            _orchardServices.ContentManager.Publish(routePart.ContentItem);
        }
        private void DeleteRoute(RoutePart routePart)
        {
            var application = GetApplication(routePart.ApplicationId);
            _applicationEvents.ApplicationUpdating(new ApplicationEventContext() { Application = application });
            _orchardServices.ContentManager.Remove(routePart.ContentItem);
        }

        public IList<RoutePart> GetRoutes(int applicationId)
        {
            var result = _orchardServices.ContentManager.Query<RoutePart, RouteRecord>()
                .Where(x => x.ApplicationId == applicationId)
                .List();

            return RoutePart.SortRouteParts(result);
        }

        public RoutePart GetRoute(int id)
        {
            return _orchardServices.ContentManager
                .Query<RoutePart, RouteRecord>()
                .Where(x => x.Id == id).List().FirstOrDefault();
        }
        public void UpdateApplication(ApplicationPart application)
        {
            var existingRoutes = GetRoutes(application.Id);
            var newRoutes = application.Routes;

            foreach (var route in newRoutes)
            {
                if (route.Id == 0)
                    CreateRoute(route as RoutePart);
                else
                    UpdateRoute(route as RoutePart);
            }
            foreach (var existingRoute in existingRoutes)
            {
                if (!newRoutes.Any(r => r.Id == existingRoute.Id)) 
                    DeleteRoute(existingRoute);
            }

            _applicationEvents.ApplicationUpdating(new ApplicationEventContext() { Application = application });
            _orchardServices.ContentManager.Publish(application.ContentItem);
            application.Routes = GetRoutes(application.Id).OfType<IRoute>().ToList();
            _applicationEvents.ApplicationUpdated(new ApplicationEventContext() { Application = application });
        }
        public void DeleteApplication(int id)
        {
            var application = GetApplication(id);
            _applicationEvents.ApplicationDeleting(new ApplicationEventContext() { Application = application });
            _orchardServices.ContentManager.Remove(application.ContentItem);
            _applicationEvents.ApplicationDeleted(new ApplicationEventContext() { Application = application });

        }

        public bool ValidateApplication(ApplicationPart application)
        {
            if (string.IsNullOrWhiteSpace(application.Name))
                return false;
            if (GetApplicationByName(application.Name) != null)
                return false;
            return true;
        }

        public void AddUserToApplication(IUser user, string application)
        {
            var applicationPart = GetApplicationByName(application).As<ApplicationPart>();

            var userApplicationsPartRecord = user.As<UserApplicationsPart>().Record;

            _userApplicationJoinRepository.Create(new UserApplicationJoinRecord()
            {
                UserApplicationsPartRecord = userApplicationsPartRecord,
                ApplicationRecord = applicationPart.Record
            });
        }

        public void RemoveUserFromApplication(IUser user, string application)
        {
            var applicationPart = GetApplicationByName(application).As<ApplicationPart>();

            var userApplicationsPartRecord = user.As<UserApplicationsPart>().Record;

            var contentApplicationRecords = _userApplicationJoinRepository.Fetch(r => r.UserApplicationsPartRecord == userApplicationsPartRecord && r.ApplicationRecord == applicationPart.Record);
            foreach (var contentApplicationRecord in contentApplicationRecords)
            {
                _userApplicationJoinRepository.Delete(contentApplicationRecord);
            }

        }

        public void UpdateUsersApplications(IUser user, List<string> applications)
        {
            var userApplicationsPartRecord = user.As<UserApplicationsPart>().Record;
            var existingApplications = userApplicationsPartRecord.Applications.Select(a => a.ApplicationRecord.Name).ToList();
            foreach (var existingApplication in existingApplications)
            {
                if (!applications.Contains(existingApplication))
                {
                    RemoveUserFromApplication(user, existingApplication);
                }
            }
            foreach (var application in applications)
            {
                if (!existingApplications.Contains(application))
                {
                    AddUserToApplication(user, application);
                }
            }
        }

    }
}