using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ceenq.com.AppRoutingServer.Models;
using ceenq.com.Apps.Models;
using ceenq.com.Apps.Services;
using ceenq.com.Apps.Validation;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Validation;
using ceenq.com.ManagementAPI.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.ManagementAPI.Controllers
{
    public class ApplicationManagementApiController : ApiController
    {
        private readonly IApplicationManager _applicationManager;
        private readonly IApplicationService _applicationService;
        private readonly IOrchardServices _orchardServices;
        private readonly INameValidator _nameValidator;
        private readonly IApplicationNameValidator _applicationNameValidator;
        public ApplicationManagementApiController(IApplicationManager applicationManager, IApplicationService applicationService, IOrchardServices orchardServices, INameValidator nameValidator, IApplicationNameValidator applicationNameValidator)
        {
            _applicationManager = applicationManager;
            _applicationService = applicationService;
            _orchardServices = orchardServices;
            _nameValidator = nameValidator;
            _applicationNameValidator = applicationNameValidator;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }


        public IHttpActionResult Get()
        {
            var applications = _applicationManager.GetApplicationParts();

            var model = applications.Select(ToModel).ToList();

            return Ok(model);
        }
        public IHttpActionResult Get(int id)
        {
            var application = _applicationManager.GetApplication(id);

            if (application == null) return BadRequest(T("Bad Request").Text);

            var model = ToModel(application);

            return Ok(model);
        }

        public IHttpActionResult Post(Application applicationModel) {

            var error = ValidateApplicationName(applicationModel.Name);
            if (error != string.Empty)
                return BadRequest(error);

            var application = _orchardServices.ContentManager.New<ApplicationPart>("Application");

            application.Name = applicationModel.Name;
            application.AuthenticationRedirect = applicationModel.AuthenticationRedirect;
            application.AccountVerification = applicationModel.AccountVerification;
            application.ResetPassword = applicationModel.ResetPassword;
            application.Domain = applicationModel.Domain;
            application.SuppressDefaultEndpoint = applicationModel.SuppressDefaultEndpoint;
            application.TransportSecurity = applicationModel.TransportSecurity;

            application.As<ApplicationRoutingServerPart>().IpAddress = applicationModel.IpAddress;

            _applicationManager.CreateApplication(application);

            return Ok(ToModel(application));
        }

        public IHttpActionResult Put(Application applicationModel) {

            var application = _applicationManager.GetApplication(applicationModel.Id);

            if (application == null)
                return BadRequest(T("Application not found").Text);

            //if the name has been changed, rerun name validation
            if (string.Compare(applicationModel.Name,application.Name,StringComparison.OrdinalIgnoreCase) != 0)
            {
                var error = ValidateApplicationName(applicationModel.Name);
                if (error != string.Empty)
                    return BadRequest(error);
            }

            application.Name = applicationModel.Name;
            application.AuthenticationRedirect = applicationModel.AuthenticationRedirect;
            application.AccountVerification = applicationModel.AccountVerification;
            application.ResetPassword = applicationModel.ResetPassword;
            application.Domain = applicationModel.Domain;
            application.SuppressDefaultEndpoint = applicationModel.SuppressDefaultEndpoint;
            application.TransportSecurity = applicationModel.TransportSecurity;


            if (applicationModel.Routes != null)
            {
                var existingRoutes = application.Routes;
                application.Routes = new List<IRoute>();
                foreach (var routeModel in applicationModel.Routes)
                {
                    var route = existingRoutes.FirstOrDefault(r => r.Id == routeModel.Id) as RoutePart ??
                                _applicationManager.NewRoute(application);

                    route.RequestPattern = routeModel.RequestPattern.Trim().ToLower();
                    route.PassTo = routeModel.PassTo.Trim().ToLower();
                    route.Rules = routeModel.Rules.Trim();
                    route.RouteOrder = routeModel.RouteOrder;
                    route.RequireAuthentication = routeModel.RequireAuthentication;
                    route.CachingEnabled = routeModel.CachingEnabled;
                    application.Routes.Add(route);
                }
            }

            //TODO: add model validation
            //  routes should have the following validations
            //  - if RoutesToExternalResource, then RequireAuthentication cannot be selected
            //  - PassTo should be a valid relative or absolute path
            //  - if $ is present, then should be a valid module

            _applicationManager.UpdateApplication(application);

            return Ok(ToModel(application));

        }

        public IHttpActionResult Delete(int id)
        {
            var application = _applicationManager.GetApplication(id);

            if (application == null)
                return BadRequest(T("Application not found").Text);

            _applicationManager.DeleteApplication(application.Id);
            return Ok();
        }

        //TODO: See if the validation can be handled by IApplicationEventHandler
        private string ValidateApplicationName(string name)
        {
            var errors = _nameValidator.Validate(name);
            if (errors.Count > 0) return errors[0];

            errors = _applicationNameValidator.Validate(name);
            if (errors.Count > 0) return errors[0];

            return string.Empty;
        }

        private Application ToModel(ApplicationPart application)
        {
            var appModel = new Application();


            appModel.Id = application.Id;
            appModel.Name = application.Name;
            appModel.Url = _applicationService.ApplicationUrl(application);
            appModel.Routes =
                _applicationManager.GetRoutes(application.Id)
                    .OrderBy(route => route.RouteOrder)
                    .Select(
                        r =>
                            new Route()
                            {
                                Id = r.Id,
                                RequestPattern = r.RequestPattern,
                                PassTo = r.PassTo,
                                RouteOrder = r.RouteOrder,
                                RequireAuthentication = r.RequireAuthentication,
                                CachingEnabled = r.CachingEnabled,
                                Rules = r.Rules
                            })
                    .ToList();
            appModel.AuthenticationRedirect = application.AuthenticationRedirect;
            appModel.AccountVerification = application.AccountVerification;
            appModel.ResetPassword = application.ResetPassword;
            appModel.Domain = application.Domain;
            appModel.SuppressDefaultEndpoint = application.SuppressDefaultEndpoint;
            appModel.TransportSecurity = application.TransportSecurity;
            appModel.IpAddress = application.As<ApplicationRoutingServerPart>().IpAddress;

            return appModel;
        }
    }
}
