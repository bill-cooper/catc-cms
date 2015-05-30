using System;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using Orchard.Localization;
using System.Web.Mvc;
using System.Web.Security;
using Orchard.Logging;
using Orchard;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Users.Services;
using Orchard.ContentManagement;
using Orchard.Users.Models;
using Orchard.UI.Notify;
using Orchard.Users.Events;
using Orchard.Utility.Extensions;
using System.Web;
using Orchard.Environment.Configuration;
using Orchard.Services;

namespace ceenq.com.Users.Controllers
{
    /// <summary>
    /// This controller is used to perform standard orchard auth to the backend and cnq account verification
    /// </summary>
    [HandleError]
    public class BackendUserController : Controller
    {
        private readonly ShellSettings _settings;
        private readonly IClock _clock;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        private readonly IUserEventHandler _userEventHandler;

        public BackendUserController(
            ShellSettings settings, 
            IClock clock, 
            IAuthenticationService authenticationService,
            IMembershipService membershipService,
            IUserService userService,
            IOrchardServices orchardServices,
            IUserEventHandler userEventHandler)
        {
            _settings = settings;
            _clock = clock;
            _authenticationService = authenticationService;
            _membershipService = membershipService;
            _userService = userService;
            _orchardServices = orchardServices;
            _userEventHandler = userEventHandler;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }


        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        public TimeSpan ExpirationTimeSpan { get; set; }


        int MinPasswordLength
        {
            get
            {
                return _membershipService.GetSettings().MinRequiredPasswordLength;
            }
        }

        [AlwaysAccessible]
        public ActionResult AccessDenied()
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
            {
                Logger.Information("Access denied to anonymous request on {0}", returnUrl);
                var shape = _orchardServices.New.LogOn().Title(T("Access Denied").Text);
                return new ShapeResult(this, shape);
            }

            //TODO: (erikpo) Add a setting for whether or not to log access denieds since these can fill up a database pretty fast from bots on a high traffic site
            //Suggestion: Could instead use the new AccessDenined IUserEventHandler method and let modules decide if they want to log this event?
            Logger.Information("Access denied to user #{0} '{1}' on {2}", currentUser.Id, currentUser.UserName, returnUrl);

            _userEventHandler.AccessDenied(currentUser);

            return View();
        }

        [AlwaysAccessible]
        public ActionResult LogOn()
        {
            if (_authenticationService.GetAuthenticatedUser() != null)
                return Redirect("~/");

            var shape = _orchardServices.New.LogOn().Title(T("Log On").Text);
            return new ShapeResult(this, shape);
        }

        [HttpPost]
        [AlwaysAccessible]
        [ValidateInput(false)]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userNameOrEmail, string password, string returnUrl, bool rememberMe = false)
        {
            var user = ValidateLogOn(userNameOrEmail, password);
            if (!ModelState.IsValid)
            {
                var shape = _orchardServices.New.LogOn().Title(T("Log On").Text);
                return new ShapeResult(this, shape);
            }

            _authenticationService.SignIn(user, rememberMe);
            _userEventHandler.LoggedIn(user);

            return this.RedirectLocal(returnUrl);
        }

        private IUser ValidateLogOn(string userNameOrEmail, string password)
        {
            bool validate = true;

            if (String.IsNullOrEmpty(userNameOrEmail))
            {
                ModelState.AddModelError("userNameOrEmail", T("You must specify a username or e-mail."));
                validate = false;
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", T("You must specify a password."));
                validate = false;
            }

            if (!validate)
                return null;

            var user = _membershipService.ValidateUser(userNameOrEmail, password);
            if (user == null)
            {
                ModelState.AddModelError("_FORM", T("The username or e-mail or password provided is incorrect."));
            }

            return user;
        }

        public ActionResult LogOff()
        {
            var wasLoggedInUser = _authenticationService.GetAuthenticatedUser();
            _authenticationService.SignOut();
            if (wasLoggedInUser != null)
                _userEventHandler.LoggedOut(wasLoggedInUser);
            return this.RedirectLocal("~/admin");
        }


        [AlwaysAccessible]
        public ActionResult RequestLostPassword()
        {
            // ensure users can request lost password
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.EnableLostPassword)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [AlwaysAccessible]
        public ActionResult RequestLostPassword(string username)
        {
            // ensure users can request lost password
            var registrationSettings = _orchardServices.WorkContext.CurrentSite.As<RegistrationSettingsPart>();
            if (!registrationSettings.EnableLostPassword)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("userNameOrEmail", T("Invalid username or E-mail."));
                return View();
            }

            var siteUrl = _orchardServices.WorkContext.CurrentSite.BaseUrl;
            if (String.IsNullOrWhiteSpace(siteUrl))
            {
                siteUrl = HttpContext.Request.ToRootUrlString();
            }

            _userService.SendLostPasswordEmail(username, nonce => Url.MakeAbsolute(Url.Action("LostPassword", "Auth", new { Area = "ceenq.com.Users", nonce = nonce }), siteUrl));

            _orchardServices.Notifier.Information(T("Check your e-mail for the confirmation link."));

            return RedirectToAction("LogOn");
        }


        [Authorize]
        [AlwaysAccessible]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MinPasswordLength;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AlwaysAccessible]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            ViewData["PasswordLength"] = MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                var validated = _membershipService.ValidateUser(User.Identity.Name, currentPassword);

                if (validated != null)
                {
                    _membershipService.SetPassword(validated, newPassword);
                    _userEventHandler.ChangedPassword(validated);
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError("_FORM",
                                         T("The current password is incorrect or the new password is invalid."));
                return ChangePassword();
            }
            catch
            {
                ModelState.AddModelError("_FORM", T("The current password is incorrect or the new password is invalid."));
                return ChangePassword();
            }
        }

        [Authorize]
        [AlwaysAccessible]
        public ActionResult SetPassword()
        {
            ViewData["PasswordLength"] = MinPasswordLength;

            return View();
        }

        [Authorize]
        [HttpPost]
        [AlwaysAccessible]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult SetPassword(string newPassword, string confirmPassword)
        {
            ViewData["PasswordLength"] = MinPasswordLength;

            try
            {
                var user = _membershipService.GetUser(User.Identity.Name);

                if (user != null)
                {
                    _membershipService.SetPassword(user, newPassword);
                    _userEventHandler.ChangedPassword(user);
                    _authenticationService.SignOut();
                    return RedirectToAction("SetPasswordSuccess");
                }

                ModelState.AddModelError("_FORM",
                                         T("The new password is invalid."));
                return ChangePassword();
            }
            catch
            {
                ModelState.AddModelError("_FORM", T("The new password is invalid."));
                return ChangePassword();
            }
        }


        [AlwaysAccessible]
        public ActionResult SetPasswordSuccess()
        {
            return View();
        }


        public ActionResult ChallengeEmail(string nonce)
        {
            var user = _userService.ValidateChallenge(nonce);

            if (user != null)
            {
                _userEventHandler.ConfirmedEmail(user);
                _authenticationService.SignIn(user,false);
                _userEventHandler.LoggedIn(user);
                return RedirectToAction("SetPassword");
            }

            return RedirectToAction("ChallengeEmailFail");
        }


        #region Validation Methods
        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", T("You must specify a current password."));
            }
            if (newPassword == null || newPassword.Length < MinPasswordLength)
            {
                ModelState.AddModelError("newPassword", T("You must specify a new password of {0} or more characters.", MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", T("The new password and confirmation password do not match."));
            }

            return ModelState.IsValid;
        }

        #endregion
    }
}