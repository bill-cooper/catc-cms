using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ceenq.com.Accounts.ViewModels;
using ceenq.com.Apps.Models;
using ceenq.com.Apps.Services;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Tenants;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace ceenq.com.Accounts.Controllers
{
    [ValidateInput(false), Admin]
    public class ApplicationController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IServerCommandProvider _serverCommandProvider;
        private readonly ITenantContextProvider _tenantContextProvider;

        public ApplicationController(
             IOrchardServices orchardServices, IServerCommandProvider serverCommandProvider, ITenantContextProvider tenantContextProvider)
        {
            _orchardServices = orchardServices;
            _serverCommandProvider = serverCommandProvider;
            _tenantContextProvider = tenantContextProvider;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Index(string accountName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var applicationService = context.Resolve<IApplicationService>();
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                var applications = applicationManager.GetApplications(includeDynamicApplications: true);

                var model = new ApplicationsIndexViewModel
                {
                    AccountName = accountName,
                    Rows = applications.Select(application =>
                    {
                        var appViewModel = new ApplicationViewModel
                        {
                            Id = application.Id,
                            Name = application.Name,
                            Url = applicationService.ApplicationUrl(application),
                            IpAddress = application.IpAddress
                        };

                        var routingServer = routingServerManager.Get(application.IpAddress);
                        appViewModel.RoutingServer = routingServer != null ? routingServer.DnsName : T("Routing Server Does Not Exist").Text;

                        return appViewModel;

                    }).ToList()
                };

                return View(model);
            }
        }



        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST(string accountName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                foreach (string key in Request.Form.Keys)
                {
                    if (key.StartsWith("Checkbox.") && Request.Form[key] == "true")
                    {
                        int applicationId = Convert.ToInt32(key.Substring("Checkbox.".Length));
                        applicationManager.DeleteApplication(applicationId);
                    }
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddRoute()
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            var model = new RouteViewModel();

            return View(model);
        }

        [HttpPost, ActionName("AddRoute")]
        public ActionResult AddRoutePOST(string accountName, int applicationId)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                var viewModel = new RouteViewModel();
                TryUpdateModel(viewModel);

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var application = applicationManager.GetApplication(applicationId);

                var route = applicationManager.NewRoute(application);

                route.RequestPattern = viewModel.RequestPattern;
                route.RequireAuthentication = viewModel.RequireAuthentication;
                route.CachingEnabled = viewModel.CachingEnabled;
                route.PassTo = viewModel.PassTo;
                route.Rules = viewModel.Rules;
                route.RouteOrder = viewModel.RouteOrder;

                // Save
                applicationManager.UpdateApplication(application);

                return RedirectToAction("Edit", new { id = application.Id });
            }
        }

        public ActionResult EditRoute(string accountName, int applicationId, int routeId)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                var route = applicationManager.GetRoute(routeId);

                var viewModel = new RouteViewModel
                {
                    RequestPattern = route.RequestPattern,
                    RequireAuthentication = route.RequireAuthentication,
                    CachingEnabled = route.CachingEnabled,
                    PassTo = route.PassTo,
                    Rules = route.Rules,
                    RouteOrder = route.RouteOrder
                };

                return View(viewModel);
            }
        }


        [HttpPost, ActionName("EditRoute")]
        public ActionResult EditRoutePOST(string accountName, int applicationId, int routeId)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                var viewModel = new RouteViewModel();
                TryUpdateModel(viewModel);

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var application = applicationManager.GetApplication(applicationId);

                var route = application.Routes.FirstOrDefault(r => r.Id == routeId) as RoutePart;

                route.RequestPattern = viewModel.RequestPattern;
                route.RequireAuthentication = viewModel.RequireAuthentication;
                route.CachingEnabled = viewModel.CachingEnabled;
                route.PassTo = viewModel.PassTo;
                route.Rules = viewModel.Rules;
                route.RouteOrder = viewModel.RouteOrder;

                // Save
                applicationManager.UpdateApplication(application);

                return RedirectToAction("Edit", new { id = applicationId });
            }
        }

        public ActionResult Create(string accountName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var contentManager = context.Resolve<IContentManager>();

                var application = contentManager.New<ApplicationPart>("Application");
                if (application == null)
                    return HttpNotFound();

                var model = contentManager.BuildEditor(application);
                return View(model);
            }
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(string accountName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();


            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var transactionManager = context.Resolve<ITransactionManager>();
                var contentManager = context.Resolve<IContentManager>();

                var application = contentManager.New<ApplicationPart>("Application");

                var model = contentManager.UpdateEditor(application, this);

                var existingApplication = applicationManager.GetApplicationByName(application.Name);
                if (existingApplication != null)
                {
                    ModelState.AddModelError("Name", T("ApplicationRecord with same name already exists"));
                }

                if (!ModelState.IsValid)
                {
                    transactionManager.Cancel();
                    return View(model);
                }

                applicationManager.CreateApplication(application);
                return RedirectToAction("Index");
            }
        }

        public ActionResult ViewConfig(string accountName, string applicationName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                    T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var routingServerManager = context.Resolve<IRoutingServerManager>();
                var routingServerConfigService = context.Resolve<IRoutingServerConfigService>();

                var application = applicationManager.GetApplicationByName(applicationName);
                if (application == null)
                {
                    return HttpNotFound();
                }

                var routingServer = routingServerManager.Get(application.IpAddress);
                var client = routingServerManager.GetCommandClient(routingServer);

                var configFile = string.Format("{0}.{1}.conf", application.Name, accountName);

                var result = client.ExecuteCommand(_serverCommandProvider.New<IGetFileCommand>(configFile));


                var generatedConfig = routingServerConfigService.GenerateConfig(application);

                var model = new AppConfigViewModel
                {
                    GeneratedText = generatedConfig,
                    ServerText = result.Message
                };
                return View(model);
            }
        }


        [HttpPost, ActionName("ViewConfig")]
        public ActionResult ViewConfigPOST(string accountName, string applicationName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var routingServerManager = context.Resolve<IRoutingServerManager>();
                var routingServerConfigManager = context.Resolve<IRoutingServerConfigManager>();

                var viewModel = new AppConfigViewModel();
                TryUpdateModel(viewModel);

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var application = applicationManager.GetApplicationByName(applicationName);
                var routingServer = routingServerManager.Get(application.IpAddress);

                routingServerConfigManager.SaveConfig(application, routingServer, viewModel.ServerText);

                _orchardServices.Notifier.Information(T("Modification to the config has been saved."));
                return View(viewModel);
            }
        }

        public ActionResult ViewLog(string accountName, string applicationName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                    T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var applicationLogProvider = context.Resolve<IApplicationLogProvider>();

                var application = applicationManager.GetApplicationByName(applicationName);
                if (application == null)
                {
                    return HttpNotFound();
                }

                var model = new AppLogViewModel
                {
                    ApplicationId = application.Id,
                    LogFile = applicationLogProvider.LogFor(application)
                };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Clear(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage config files")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                var application = applicationManager.GetApplication(id);
                if (application == null)
                {
                    return HttpNotFound();
                }

                var routingServer = routingServerManager.Get(application.As<IApplicationRoutingServer>().IpAddress);
                var client = routingServerManager.GetCommandClient(routingServer);

                client.ExecuteCommand(_serverCommandProvider.New<ITruncateFileCommand>("/var/log/nginx/error.log"));

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        [HttpPost]
        public ActionResult Refresh(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner,
                    T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();
                var applicationLogProvider = context.Resolve<IApplicationLogProvider>();

                var application = applicationManager.GetApplication(id);
                if (application == null)
                {
                    return HttpNotFound();
                }

                var model = new AppLogViewModel
                {
                    ApplicationId = application.Id,
                    LogFile = applicationLogProvider.LogFor(application)
                };

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        public ActionResult Edit(string accountName, int id)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                var application = applicationManager.GetApplication(id);
                if (application == null)
                {
                    return HttpNotFound();
                }

                var model = new ApplicationEditViewModel
                {
                    Id = application.Id,
                    Name = application.Name,
                    Routes = applicationManager.GetRoutes(application.Id).Select(r => new RouteViewModel()
                    {
                        Id = r.Id,
                        RequestPattern = r.RequestPattern,
                        PassTo = r.PassTo,
                        RouteOrder = r.RouteOrder,
                        RequireAuthentication = r.RequireAuthentication,
                        CachingEnabled = r.CachingEnabled,
                        Rules = r.Rules
                    }).ToList(),
                    AuthenticationRedirect = application.AuthenticationRedirect,
                    AccountVerification = application.AccountVerification,
                    ResetPassword = application.ResetPassword,
                    Domain = application.Domain,
                    SuppressDefaultEndpoint = application.SuppressDefaultEndpoint,
                    TransportSecurity = application.TransportSecurity

                };

                return View(model);
            }
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditSavePOST(string accountName, int id)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                var viewModel = new ApplicationEditViewModel();
                TryUpdateModel(viewModel);

                if (String.IsNullOrEmpty(viewModel.Name))
                {
                    ModelState.AddModelError("Name", T("ApplicationRecord name can't be empty"));
                }

                var existingApplication = applicationManager.GetApplicationByName(viewModel.Name);
                if (existingApplication != null && existingApplication.Id != id)
                {
                    ModelState.AddModelError("Name", T("ApplicationRecord with same name already exists"));
                }

                if (!ModelState.IsValid)
                {
                    return Edit(accountName, id);
                }

                var application = applicationManager.GetApplication(viewModel.Id);

                application.Name = viewModel.Name;
                application.AuthenticationRedirect = viewModel.AuthenticationRedirect;
                application.AccountVerification = viewModel.AccountVerification;
                application.ResetPassword = viewModel.ResetPassword;
                application.Domain = viewModel.Domain;
                application.SuppressDefaultEndpoint = viewModel.SuppressDefaultEndpoint;
                application.TransportSecurity = viewModel.TransportSecurity;

                if (viewModel.Routes != null)
                {
                    var existingRoutes = application.Routes;
                    application.Routes = new List<IRoute>();
                    foreach (var routeModel in viewModel.Routes)
                    {
                        var route = existingRoutes.FirstOrDefault(r => r.Id == routeModel.Id) as RoutePart ??
                                    applicationManager.NewRoute(application);

                        route.RequestPattern = routeModel.RequestPattern;
                        route.PassTo = routeModel.PassTo;
                        route.Rules = routeModel.Rules;
                        route.RouteOrder = routeModel.RouteOrder;
                        route.RequireAuthentication = routeModel.RequireAuthentication;
                        route.CachingEnabled = routeModel.CachingEnabled;
                        application.Routes.Add(route);
                    }
                }


                // Save
                applicationManager.UpdateApplication(application);

                _orchardServices.Notifier.Information(T("Your ApplicationRecord has been saved."));
                return RedirectToAction("Edit", new { id });
            }
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Delete")]
        public ActionResult EditDeletePOST(string accountName, int id)
        {
            return Delete(accountName, id, null);
        }

        public ActionResult DeleteRoute(string accountName, int applicationId, int routeId)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                var application = applicationManager.GetApplication(applicationId);
                application.Routes.Remove(application.Routes.FirstOrDefault(r => r.Id == routeId));
                applicationManager.UpdateApplication(application);

                _orchardServices.Notifier.Information(T("Route deleted"));
                return RedirectToAction("Edit", new { applicationId });
            }
        }

        [HttpPost]
        public ActionResult Delete(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage applications")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var applicationManager = context.Resolve<IApplicationManager>();

                applicationManager.DeleteApplication(id);
                _orchardServices.Notifier.Information(T("ApplicationRecord was successfully deleted."));

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}
