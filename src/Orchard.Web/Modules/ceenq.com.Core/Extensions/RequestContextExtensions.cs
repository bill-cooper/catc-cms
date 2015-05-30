using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ceenq.com.Core.Extensions
{
    public static class RequestContextExtensions
    {
        public static bool IsEmbedded(this RequestContext requestContext)
        {
            return requestContext.HttpContext.Request.Headers["embedded"] != null;
        }
    }
}