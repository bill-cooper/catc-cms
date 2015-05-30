using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using ceenq.com.Core.Api;
using ceenq.com.Core.Applications;
using Orchard;

namespace ceenq.com.ManagementAPI.Filters
{
    public class ManagementApiActionFilter : WebApiActionFilter
    {
        private readonly IApplicationRequestContext _applicationRequestContext;
        public ManagementApiActionFilter(IApplicationRequestContext applicationRequestContext)
        {
            _applicationRequestContext = applicationRequestContext;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var workContext = actionContext.ControllerContext.GetWorkContext();
            var httpContext = workContext.HttpContext;

            //only target management api calls
            if (httpContext.Request.RawUrl.StartsWith("/cnq/api/")) return;

            //if this request is not coming from the dashboard application, then 404
            if(_applicationRequestContext.Application.Name != "dashboard")
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }

    }
}