using System.Web;
using System.Web.Mvc;
using ceenq.com.Core.Applications;
using Orchard.Mvc.Filters;
using System.Web.Mvc.Filters;

namespace ceenq.com.Users.Filters
{
    public class AuthenticationRedirectionFilter : FilterProvider, IAuthenticationFilter {
        private readonly IApplicationRequestContext _applicationContext;
        public AuthenticationRedirectionFilter(IApplicationRequestContext applicationContext) {
            _applicationContext = applicationContext;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                if (!_applicationContext.IsApplicationRequest()) return; //let the default auth redirect catch non application requests

                var redirectTo = _applicationContext.Application.AuthenticationRedirect;
                var requestUrl = _applicationContext.ApplicationRequestUrl();
                if (!string.IsNullOrEmpty(requestUrl))
                {
                    redirectTo = string.Format(redirectTo + "?returnUrl={0}", HttpUtility.UrlEncode(requestUrl));
                }
                filterContext.Result = new RedirectResult(redirectTo);


            }
        }
    }
}