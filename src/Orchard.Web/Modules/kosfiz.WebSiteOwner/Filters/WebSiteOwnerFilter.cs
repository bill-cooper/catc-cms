using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Mvc.Filters;
using System.Web.Mvc;
using Orchard;
using Orchard.Caching;
using kosfiz.WebSiteOwner.Services;
using Orchard.UI.Admin;
using Orchard.UI.Resources;

namespace kosfiz.WebSiteOwner.Filters
{
    public class WebSiteOwnerFilter: FilterProvider, IResultFilter
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IWebSiteOwnerService _webSiteOwnerService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public WebSiteOwnerFilter(
            IWorkContextAccessor workContextAccessor,
            IWebSiteOwnerService webSiteOwnerService,
            ICacheManager cacheManager,
            ISignals signals) {
            _workContextAccessor = workContextAccessor;
            _webSiteOwnerService = webSiteOwnerService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (AdminFilter.IsApplied(filterContext.RequestContext))
                return;
            
            IResourceManager resourceManager = _workContextAccessor.GetContext().Resolve<IResourceManager>();

            var metas = _cacheManager.Get("kosfiz.WebSiteOwner", ctx =>
            {
                ctx.Monitor(_signals.When("kosfiz.WebSiteOwnerRecordChanged"));
                var _metas = _webSiteOwnerService.Get();
                return _metas;
            });

            foreach (var item in metas)
                resourceManager.SetMeta(new MetaEntry { Name = item.MetaName, Content = item.MetaContent });
        }
    }
}