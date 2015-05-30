using System.Web.Mvc;
using ceenq.com.Accounts.ViewModels;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Tenants;
using ceenq.com.RoutingServer.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace ceenq.com.Accounts.Controllers
{
    [ValidateInput(false), Admin]
    public class RoutingServerController : Controller
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IServerCommandProvider _serverCommandProvider;
        private readonly ITenantContextProvider _tenantContextProvider;
        public RoutingServerController(IOrchardServices orchardServices, IServerCommandProvider serverCommandProvider, ITenantContextProvider tenantContextProvider)
        {
            _orchardServices = orchardServices;
            _serverCommandProvider = serverCommandProvider;
            _tenantContextProvider = tenantContextProvider;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        public ActionResult Index(string accountName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();
                var model = new RoutingServerIndexViewModel
                {
                    AccountName = accountName,
                    Rows = routingServerManager.List()
                };
                    
                return View(model);
            }
        }


        public ActionResult Create()
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(string accountName)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();
                routingServerManager.New();

                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult Delete(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                routingServerManager.Delete(id);

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        public ActionResult Log(string accountName, string ipAddress)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage log files")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                var commandClient = routingServerManager.GetCommandClient(ipAddress);
                var fileInfo =
                    commandClient.ExecuteCommand(
                        _serverCommandProvider.New<IGetFileInfoCommand>("/var/log/nginx/error.log")).Message;

                var parts = fileInfo.Split(' ');
                long fileSize = 0;
                long.TryParse(parts[4], out fileSize);

                var model = new LogViewModel()
                {
                    AccountName = accountName,
                    IpAddress = ipAddress
                };

                if (fileSize > Constants.MB * 2)
                {
                    model.LogText = "The log is too big to load.  Clear log first.";
                }
                else
                {
                    var result =
                        commandClient.ExecuteCommand(
                            _serverCommandProvider.New<IReadFileCommand>("/var/log/nginx/error.log", "1000"));
                    model.LogText = result.Message;
                }

                return View(model);
            }
        }



        [HttpPost]
        public ActionResult Clear(string accountName, string ipAddress, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();
                var commandClient = routingServerManager.GetCommandClient(ipAddress);
                commandClient.ExecuteCommand(_serverCommandProvider.New<ITruncateFileCommand>("/var/log/nginx/error.log"));

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        [HttpPost]
        public ActionResult RestartWebServer(string accountName, string ipAddress, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                var commandClient = routingServerManager.GetCommandClient(ipAddress);
                try
                {
                    commandClient.ExecuteCommand(_serverCommandProvider.New<INginxRestartCommand>());
                    _orchardServices.Notifier.Information(T("Routing Server Restarted"));
                }
                catch (ServerCommandException ex)
                {
                    _orchardServices.Notifier.Error(ex.LocalizedMessage);
                }

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        [HttpPost]
        public ActionResult RestartVm(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                routingServerManager.Restart(id);

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }
        [HttpPost]
        public ActionResult PowerOnVm(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                routingServerManager.PowerOn(id);

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }
        [HttpPost]
        public ActionResult PowerOffVm(string accountName, int id, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                routingServerManager.PowerOff(id);

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        [HttpPost]
        public ActionResult Refresh(string accountName, string ipAddress, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage log files")))
                return new HttpUnauthorizedResult();

            using (var context = _tenantContextProvider.ContextFor(accountName))
            {
                var routingServerManager = context.Resolve<IRoutingServerManager>();

                var commandClient = routingServerManager.GetCommandClient(ipAddress);

                var result =
                    commandClient.ExecuteCommand(_serverCommandProvider.New<IGetFileCommand>("/var/log/nginx/error.log"));

                var model = new LogViewModel
                {
                    IpAddress = ipAddress,
                    LogText = result.Message
                };

                return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
            }
        }

        [HttpPost]
        public ActionResult ExecuteCommands(string name, string returnUrl)
        {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage routing servers")))
                return new HttpUnauthorizedResult();

            //TODO: Complete This
            //var client = _serverCommandClientProvider.Connect(new SshConnectionInfo()
            //{
            //    Host = "",
            //    Port = 0,
            //    Username = "",
            //    Password = ""
            //});



            //return View("Index", model);


            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

    }
}