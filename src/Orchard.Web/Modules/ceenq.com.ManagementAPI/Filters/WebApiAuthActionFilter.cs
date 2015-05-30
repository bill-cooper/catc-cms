using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ceenq.com.Core.Api;
using Orchard;
using Orchard.Security;

namespace ceenq.com.ManagementAPI.Filters
{
    /// <summary>
    /// This is not a functional filter, testing
    /// </summary>
    public class WebApiAuthActionFilter : WebApiActionFilter
    {
        private readonly IAuthenticationService _authenticationService;
        public WebApiAuthActionFilter(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var workContext = actionContext.ControllerContext.GetWorkContext();
            var httpContext = workContext.HttpContext;

            if (httpContext.Request.Path.ToLower() != "/cnq/api/auth/signin" && !httpContext.User.Identity.IsAuthenticated)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            //var user = _authenticationService.GetAuthenticatedUser();;
            //if(!user.As<IUserRoles>().Roles.Any(role => role == "AccountAdmin"))
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
        }
    }
}