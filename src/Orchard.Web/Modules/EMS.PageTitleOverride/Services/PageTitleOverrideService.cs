using System;
using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.Caching;
using Orchard.Environment.Extensions;
using EMS.PageTitleOverride.Models;
using System.Web;

namespace EMS.PageTitleOverride.Services {
    
    public class PageTitleOverrideService : IPageTitleOverrideService {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        
        public PageTitleOverrideService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals) {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public PageTitleOverrideService(WorkContext workContext) {
            _wca = workContext.Resolve<IWorkContextAccessor>();
            _cacheManager = new DefaultCacheManager(this.GetType(), new DefaultCacheHolder(new DefaultCacheContextAccessor()));
            _signals = workContext.Resolve<ISignals>();
        }

        public bool GetIsPageTitleSiteNameLast() {
            return bool.Parse(
                _cacheManager.Get(
                    "EMS.PageTitleOverride.IsPageTitleSiteNameLast",
                    ctx => {
                        ctx.Monitor(_signals.When("EMS.PageTitleOverride.Changed"));
                        WorkContext workContext = _wca.GetContext();
                        var pageTitleOverrideSettings =
                            (PageTitleOverrideSettingsPart) workContext
                                .CurrentSite
                                .ContentItem
                                .Get(typeof(PageTitleOverrideSettingsPart));
                        return pageTitleOverrideSettings.IsPageTitleSiteNameLast.ToString();
                    })
                );
        }
        
        public bool GetIsPageTitleHideSiteName() {
            return bool.Parse(
                _cacheManager.Get(
                    "EMS.PageTitleOverride.IsPageTitleHideSiteName",
                    ctx => {
                        ctx.Monitor(_signals.When("EMS.PageTitleOverride.Changed"));
                        WorkContext workContext = _wca.GetContext();
                        var pageTitleOverrideSettings =
                            (PageTitleOverrideSettingsPart)workContext
                                .CurrentSite
                                .ContentItem
                                .Get(typeof(PageTitleOverrideSettingsPart));
                        return pageTitleOverrideSettings.IsPageTitleHideSiteName.ToString();
                    })
                );
        }

        public string GetPageTitleOverride() {
            string pageTitleOverride = "";
            try {
                if (HttpContext.Current.Cache["EMS.PageTitleOverride.PageTitle"] != null) {
                    pageTitleOverride = HttpContext.Current.Cache["EMS.PageTitleOverride.PageTitle"].ToString();
                    HttpContext.Current.Cache["EMS.PageTitleOverride.PageTitle"] = "";
                }
            }
            catch { }
            return pageTitleOverride;
        }

    }
}