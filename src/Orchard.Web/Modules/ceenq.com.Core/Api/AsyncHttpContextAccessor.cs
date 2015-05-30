using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using ceenq.com.Core.Environment;
using Orchard.Mvc;

namespace ceenq.com.Core.Api
{
    public class AsyncHttpContextAccessor : IHttpContextAccessor
    {
        private HttpContextBase _stub;

        public HttpContextBase Current()
        {
            var httpContext = GetStaticProperty() ?? ThreadSafeHttpContextAccessor.GetContext();
            return httpContext != null ? new HttpContextWrapper(httpContext) : _stub;
        }

        public void Set(HttpContextBase stub)
        {
            _stub = stub;
        }

        private HttpContext GetStaticProperty()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return null;
            }

            try
            {
                if (httpContext.Request == null)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return httpContext;
        }
    }
    public class AsyncHttpContextAccessorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AsyncHttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }
    }
}