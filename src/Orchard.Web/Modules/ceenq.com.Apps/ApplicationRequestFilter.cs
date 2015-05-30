using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Security;
using ceenq.com.Core.Utility;
using Orchard;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Filters;

namespace ceenq.com.Apps
{
    public class ApplicationRequestFilter : FilterProvider, IActionFilter
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationAuthorizationService _applicationAuthorizationService;
        private readonly IApplicationService _applicationService;
        private readonly IApplicationRequestContext _applicationContext;
        private readonly IAccountContext _accountContext;
        private readonly WorkContext _workContext;
        private readonly IAliasService _aliasService;

        public ApplicationRequestFilter(
            IApplicationAuthorizationService accountAuthorizationService,
            IApplicationRequestContext applicationContext,
            IOrchardServices orchardServices,
            IWorkContextAccessor workContextAccessor,
            IAccountContext accountContext,
            IApplicationService applicationService,
            IAliasService aliasService)
        {
            _applicationAuthorizationService = accountAuthorizationService;
            _applicationContext = applicationContext;
            _orchardServices = orchardServices;
            _accountContext = accountContext;
            _applicationService = applicationService;
            _aliasService = aliasService;
            _workContext = workContextAccessor.GetContext();
            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var requestPath = filterContext.RequestContext.HttpContext.Request.Path.ToLower();

            //let request to base directory pass through.  This request will be redirected
            // to /admin by redirectcontroller
            if (requestPath == "/")
                return;

            //let admin routes pass through
            if (requestPath.StartsWith("/admin/") || requestPath == "/admin")
                return;

            //let Orchard.MediaLibrary routes pass through
            if (requestPath.StartsWith("/orchard.medialibrary/"))
                return;

            //let Orchard gallary routes pass through
            if (requestPath.StartsWith("/packaging/"))
                return;

            //allow admin access 
            if (requestPath.StartsWith("/users/account/") || requestPath.StartsWith("/users/validate"))
                return;

            //allow submission of dynamic forms 
            if (requestPath.StartsWith("/orchard.dynamicforms"))
                return;

            //allow access to audit trail
            if (requestPath.StartsWith("/orchard.audittrail"))
                return;

            //allow access to projection
            if (requestPath.StartsWith("/projector"))
                return;


            if (_applicationContext.IsApplicationRequest())
            {
                //if an asset request has been directed through the .net request pipeline then
                // we know it was for the reason that authentication is required.  So, if the user
                // is not authenticated, then do a redirect to the established login page
                if (_workContext.CurrentUser == null && _applicationContext.IsAssetRequest())
                {
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                    var redirectTo = _applicationContext.Application.AuthenticationRedirect;
                    var requestUrl = _applicationContext.ApplicationRequestUrl();
                    if (!string.IsNullOrEmpty(requestUrl))
                    {
                        redirectTo = string.Format(redirectTo + "?returnUrl={0}", HttpUtility.UrlEncode(requestUrl));
                    }
                    filterContext.Result = new RedirectResult(redirectTo);
                    return;
                }
                else if (_workContext.CurrentUser != null && !_applicationAuthorizationService.TryCheckAccess(_workContext.CurrentUser, _applicationContext.Application))
                {
                    filterContext.RequestContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                var httpContext = _workContext.HttpContext;
                if (_applicationContext.IsCmsRequest())
                {
                    // the reason for doing the Server.TransferRequest here is because all request coming in from 
                    // proxy server will have a url path starting with cnq/cms.  This is done to establish an
                    // isolated url space for requests to orchard that are meant to service cnq applications.
                    // this approach probably needs to be rethought.
                    if (!IsTransfer(httpContext))
                    {
                        var url = _workContext.HttpContext.Request.RawUrl.TrimStart('/');
                        //REFACTOR: this may not be the best place to resolve to index.html
                        //if the request is to a directory, then direct to index


                        url = _applicationService.ToPublicApplicationPath(_applicationContext.Application, url);
                        var cmsPath = _accountContext.ToPublicCmsPath(url);

                        //if there is not an alias set up for this path, then treat the request as a directory, and direct to index.html 
                        if (url.EndsWith("/"))
                        {
                            if (_aliasService.Get(cmsPath.TrimStart('/')) == null)
                            {
                                cmsPath = cmsPath + "/index.html";
                            }
                        }



                        httpContext.Server.TransferRequest(cmsPath, true);
                    }
                    return;
                }

            }

            //since this is not an application request, then return 404 not found
            // no other request except the ones listed above are valid
            RespondNotFound(filterContext);
        }

        private bool IsTransfer(HttpContextBase context)
        {
            //check to see if this is a transferred request

            //REFACTOR: This is a very poor way of determining if this is a request that has gone through
            // a Server.TransferRequest.  It's the only way I could figure out though.  The whole approach
            // for service CMS content needs to be rethought.
            var rawUrl = context.Request.RawUrl;
            if (context.Request.Url != null && !string.IsNullOrWhiteSpace(context.Request.Url.Query))
                rawUrl = rawUrl.Replace(context.Request.Url.Query, "");

            //if it is not, then the rawurl and path should match
            return PathHelper.CleanPath(rawUrl) != PathHelper.CleanPath(context.Request.Path);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        private void RespondNotFound(ActionExecutingContext filterContext)
        {

            var request = filterContext.RequestContext.HttpContext.Request;
            var url = filterContext.RequestContext.HttpContext.Request.RawUrl.ToLower();
            var model = _orchardServices.New.NotFound();

            // If the url is relative then replace with Requested path
            model.RequestedUrl = request.Url.OriginalString.Contains(url) & request.Url.OriginalString != url ?
                request.Url.OriginalString : url;

            // Dont get the user stuck in a 'retry loop' by
            // allowing the Referrer to be the same as the Request
            model.ReferrerUrl = request.UrlReferrer != null &&
                request.UrlReferrer.OriginalString != model.RequestedUrl ?
                request.UrlReferrer.OriginalString : null;

            filterContext.Result = new ShapeResult(filterContext.Controller, model);
            filterContext.RequestContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

            // prevent IIS 7.0 classic mode from handling the 404/500 itself
            filterContext.RequestContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        private void TransferTo(string path)
        {
            var httpContext = _workContext.HttpContext;
            httpContext.Server.TransferRequest(path, true);
        }
    }
}