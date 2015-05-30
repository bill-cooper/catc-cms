using System;
using System.Linq;
using System.Web.Mvc;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Tenants;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using ceenq.com.Accounts.Models;
using ceenq.com.Accounts.ViewModels;
using Orchard.Security;
using Orchard.UI.Notify;

namespace ceenq.com.Accounts.Controllers
{
    [ValidateInput(false)]
    public class AdminController : Controller
    {
        private readonly IAccountManager _accountService;
        private readonly ITenantContextProvider _tenantContextProvider;
        private readonly IShellSettingsManager _shellSettingsManager;
        private readonly ShellSettings _currentShellSettings;

        public AdminController(
            IOrchardServices services,
            IAccountManager accountService, ITenantContextProvider tenantContextProvider, IShellSettingsManager shellSettingsManager, ShellSettings currentShellSettings)
        {
            Services = services;
            _accountService = accountService;
            _tenantContextProvider = tenantContextProvider;
            _shellSettingsManager = shellSettingsManager;
            _currentShellSettings = currentShellSettings;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Index()
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
                return new HttpUnauthorizedResult();

            var model = new AccountsIndexViewModel();

            var accounts = _accountService.GetAccounts().OrderBy(r => r.Name);
            foreach (var account in accounts)
            {
                var accountModel = new AccountViewModel
                {
                    Id = account.Id,
                    DisplayName = account.DisplayName,
                    Domain = account.Domain,
                    Name = account.Name,
                    ContentItem = account.As<AccountPart>().ContentItem,
                    Tenant = _shellSettingsManager.LoadSettings().FirstOrDefault(s => s.Name == account.Name)
                };
                if (_shellSettingsManager.LoadSettings().FirstOrDefault(s => s.Name == account.Name).State == TenantState.Running)
                {
                    try
                    {
                        using (var context = _tenantContextProvider.ContextFor(account.Name))
                        {
                            var routingServerManager = context.Resolve<IRoutingServerManager>();
                            var routingServer = routingServerManager.GetDefault();
                            accountModel.DefaultRoutingServer = string.Format("{0} ({1})", routingServer.DnsName,
                                routingServer.IpAddress);
                        }
                    }
                    catch (Exception ex)
                    {
                        Services.Notifier.Error(T(ex.Message));
                        Logger.Error(ex,
                            string.Format(
                                "An exception was thrown while attempting to access the context for account {0}",
                                account));
                    }
                }
                model.Rows.Add(accountModel);
            }


            return View(model);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST()
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
                return new HttpUnauthorizedResult();

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("Checkbox.") && Request.Form[key] == "true")
                {
                    int accountId = Convert.ToInt32(key.Substring("Checkbox.".Length));
                    _accountService.DeleteAccount(accountId);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
                return new HttpUnauthorizedResult();

            var model = new AccountCreateViewModel();
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST()
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
                return new HttpUnauthorizedResult();

            var viewModel = new AccountCreateViewModel();
            TryUpdateModel(viewModel);

            if (String.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Name", T("AccountRecord name can't be empty"));
            }

            var account = _accountService.GetAccountByName(viewModel.Name);
            if (account != null)
            {
                ModelState.AddModelError("Name", T("AccountRecord with same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _accountService.CreateAccount(viewModel.Name);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
                return new HttpUnauthorizedResult();

            var account = _accountService.GetAccount(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            var model = new AccountEditViewModel
            {
                Name = account.Name,
                Id = account.Id
            };

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.CreateStorageShare")]
        public ActionResult EditSavePOST(int id)
        {
            //if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
            //    return new HttpUnauthorizedResult();

            //var account = _accountService.GetAccount(id);

            //var canonicalAccountName = _accountHelper.CanonicalAccountName(account);
            //_cloudStorageManager.CreateStorage(canonicalAccountName);

            //ModelState.AddModelError("Name", T("AccountRecord cannot be modifed after creation.  Delete and recreate account."));

            //return RedirectToAction("Edit", new { id });
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Delete")]
        public ActionResult EditDeletePOST(int id)
        {
            return Delete(id, null);
        }

        [HttpPost]
        public ActionResult Delete(int id, string returnUrl)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage accounts")))
                return new HttpUnauthorizedResult();

            _accountService.DeleteAccount(id);
            Services.Notifier.Information(T("AccountRecord was successfully deleted."));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        [HttpPost]
        public ActionResult Disable(string name, string returnUrl)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Couldn't disable tenant")))
                return new HttpUnauthorizedResult();

            if (_currentShellSettings.Name != ShellSettings.DefaultName)
                return new HttpUnauthorizedResult();

            var tenant = _shellSettingsManager.LoadSettings().FirstOrDefault(ss => ss.Name == name);

            if (tenant != null && tenant.Name != _currentShellSettings.Name)
            {
                tenant.State = TenantState.Disabled;
                _shellSettingsManager.SaveSettings(tenant);
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }
        [HttpPost]
        public ActionResult Enable(string name, string returnUrl)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Couldn't disable tenant")))
                return new HttpUnauthorizedResult();

            if (_currentShellSettings.Name != ShellSettings.DefaultName)
                return new HttpUnauthorizedResult();

            var tenant = _shellSettingsManager.LoadSettings().FirstOrDefault(ss => ss.Name == name);

            if (tenant != null && tenant.Name != _currentShellSettings.Name)
            {
                tenant.State = TenantState.Running;
                _shellSettingsManager.SaveSettings(tenant);
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }
    }
}
