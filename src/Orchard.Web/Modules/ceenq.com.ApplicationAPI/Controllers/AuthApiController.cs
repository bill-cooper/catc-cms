using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Models;
using ceenq.com.Users.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Roles.Models;
using Orchard.Security;
using Orchard.Users.Events;
using Orchard.Users.Models;
using Orchard.Users.Services;

namespace ceenq.com.ApplicationAPI.Controllers
{
    /// <summary>
    /// This controller is used to do API based auth for cnq accounts
    /// </summary>
    public class AuthApiController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IUserEventHandler _userEventHandler;
        private readonly IApplicationRequestContext _applicationContext;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationService _applicationService;
        private readonly IAccountContext _accountContext;

        public AuthApiController(
            IAuthenticationService authenticationService,
            IMembershipService membershipService,
            IUserEventHandler userEventHandler,
            IApplicationRequestContext applicationContext,
            IUserService userService,
            IOrchardServices orchardServices,
            IApplicationService applicationService, 
            IAccountContext accountContext)
        {
            _authenticationService = authenticationService;
            _membershipService = membershipService;
            _userEventHandler = userEventHandler;
            _applicationContext = applicationContext;
            _userService = userService;
            _orchardServices = orchardServices;
            _applicationService = applicationService;
            _accountContext = accountContext;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        [HttpGet]
        public HttpResponseMessage AuthenticatedUser()
        {
            var loggedInUser = _authenticationService.GetAuthenticatedUser().As<UserPart>();

            //TODO: Fix this.  Users domain is being built and sent to the client.  implement a better way.
            var baseDomain = _accountContext.AccountDomain.Substring(_accountContext.AccountDomain.IndexOf('.'));
            var user = new UserAuthModel{IsAuthenticated = false};
            if (loggedInUser != null) {
                user.IsAuthenticated = true;
                user.Id = loggedInUser.Id;
                user.UserName = loggedInUser.UserName;
                user.Email = loggedInUser.Email;
                user.Account = string.Format("{0}{1}", user.UserName, baseDomain);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [HttpPost]
        public IHttpActionResult SignIn(UserAuthModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _membershipService.ValidateUser(credentials.UserName, credentials.Password);
            if (user == null)
            {
                return BadRequest(T("Invalid username or password").Text);
            }

            var userRoles = user.As<IUserRoles>();

            var applicationUserOnly = userRoles.Roles.Contains(Roles.ApplicationUser) && userRoles.Roles.Count == 1;

            //if the auth request is to the dashboard and the user is more than just an application user
            if (!applicationUserOnly && _applicationContext.Application.Name == "dashboard")
            {
                return Ok(Authenticate(user, credentials));
            }

            //if the user is associated with the application that is being requested, then pass
            var userApplications = user.As<IUserApplications>();
            if (userRoles.Roles.Contains(Roles.ApplicationUser) && userApplications.ApplicationNames.Any(appName => String.Equals(appName, _applicationContext.Application.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return Ok(Authenticate(user,credentials));
            }
            return BadRequest(T("Invalid username or password").Text);
        }

        private UserAuthModel Authenticate(IUser user, UserAuthModel credentials)
        {
            _authenticationService.SignIn(user, credentials.RememberMe);
            _userEventHandler.LoggedIn(user);

            return new UserAuthModel
            {
                IsAuthenticated = true,
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }


        [HttpPost]
        public HttpResponseMessage SignOut()
        {
            var wasLoggedInUser = _authenticationService.GetAuthenticatedUser();
            _authenticationService.SignOut();
            if (wasLoggedInUser != null)
                _userEventHandler.LoggedOut(wasLoggedInUser);

            return this.Request.CreateResponse(HttpStatusCode.Created, true);
        }

        [HttpPost]
        public IHttpActionResult RequestPassword(RequestPasswordModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return BadRequest(T("Invalid username or email").Text);
            }

            var lowerName = model.Username.ToLowerInvariant();
            var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == lowerName || u.Email == lowerName).List().FirstOrDefault();
            if (user != null) {
                //if the user is associated with the application that is being requested, then pass
                var userApplications = user.As<IUserApplications>();
                if (userApplications.ApplicationNames.Any(appName => String.Equals(appName, _applicationContext.Application.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var urlHelper = new System.Web.Mvc.UrlHelper(_orchardServices.WorkContext.HttpContext.Request.RequestContext);
                    var url = _applicationService.ToPublicAbsoluteApplicationPath(_applicationContext.Application, _applicationContext.Application.ResetPassword);
                    _userService.SendLostPasswordEmail(model.Username, nonce => string.Format("{0}?nonce={1}", url, urlHelper.Encode(nonce)));
                }
            }
            return Ok(T("A password request email has been sent."));
        }

        [HttpPost]
        public IHttpActionResult ResetPassword(ResetPasswordModel model)
        {
            IUser user;
            if ((user = _userService.ValidateLostPassword(model.Nonce)) == null)
            {
                return BadRequest(T("Invalid request").Text);
            }

            if (model.NewPassword == null || model.NewPassword.Length < 6)
            {
                return BadRequest(T("Password not long enough").Text);
            }

            if (!string.Equals(model.NewPassword, model.ConfirmPassword, StringComparison.Ordinal))
            {
                return BadRequest(T("The new password and confirmation password do not match.").Text);
            }

            if (user.As<UserPart>().EmailStatus == UserStatus.Pending)
                user.As<UserPart>().EmailStatus = UserStatus.Approved;

            _membershipService.SetPassword(user, model.NewPassword);

            _userEventHandler.ChangedPassword(user);

            return Ok(T("Password changed."));
        }

    }
}
