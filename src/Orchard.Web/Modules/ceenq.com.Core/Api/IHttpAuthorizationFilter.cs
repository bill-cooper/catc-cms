using Orchard.WebApi.Filters;

namespace ceenq.com.ManagementAPI
{
    public interface IHttpAuthorizationFilter : System.Web.Http.Filters.IAuthorizationFilter, IApiFilterProvider
    {
    }
}