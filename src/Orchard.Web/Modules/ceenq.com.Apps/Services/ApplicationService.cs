using System;
using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Utility;
using Orchard.ContentManagement;
using Orchard.Core.Settings.Models;
using Orchard.Settings;

namespace ceenq.com.Apps.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationManager _applicationManager;
        private readonly ISiteService _siteService;
        private readonly IAccountContext _accountContext;
        private readonly IApplicationDnsNamesService _applicationDnsNamesService;

        public ApplicationService(ISiteService siteService, IApplicationManager applicationManager, IAccountContext accountContext, IApplicationDnsNamesService applicationDnsNamesService)
        {
            _siteService = siteService;
            _applicationManager = applicationManager;
            _accountContext = accountContext;
            _applicationDnsNamesService = applicationDnsNamesService;
        }

        public string ToInternalApplicationPath(IApplication application, string path)
        {
            return string.Format("{0}{1}", BaseDirectory(application), PathHelper.CleanPath(path)).Replace("//", "/");
        }

        public string ToInternalAbsoluteApplicationPath(IApplication application, string path)
        {
            var baseUrl = _siteService.GetSiteSettings().As<SiteSettingsPart>().BaseUrl;
            return string.Format("{0}/{1}", baseUrl, ToInternalApplicationPath(application, path));
        }

        public string ToPublicApplicationPath(IApplication application, string path)
        {
            path = PathHelper.CleanPath(path);

            path = PathHelper.CleanPath(path);

            if (path.StartsWith(_accountContext.AssetPath, StringComparison.OrdinalIgnoreCase))
                path = path.Substring(_accountContext.AssetPath.Length);
            
            if (path.StartsWith(BaseDirectory(application), StringComparison.OrdinalIgnoreCase))
                path = path.Substring(BaseDirectory(application).Length);

            return PathHelper.CleanPath(path);
        }

        public string ToPublicAbsoluteApplicationPath(IApplication application, string path)
        {
            return string.Format("{0}{1}", ApplicationUrl(application), ToPublicApplicationPath(application, path).TrimStart(new[] { '/' }));
        }

        public string ApplicationUrl(IApplication application)
        {
            var protocal = application.TransportSecurity ? "https" : "http";
            return string.Format("{0}://{1}/",protocal, _applicationDnsNamesService.PrimaryDnsName(application));
        }

        public IList<IRoute> GetRoutes(IApplication application)
        {
            return _applicationManager.GetRoutes(application.Id).OfType<IRoute>().ToList();
        }

        /// <summary>
        /// Provides the base directory for which this application should route
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        public string BaseDirectory(IApplication application)
        {
            var routes = GetRoutes(application);
            var baseRoute = routes.FirstOrDefault(r => r.RequestPattern.Trim() == "/");
            if (baseRoute == null) return "/";
            return baseRoute.PassTo;
        }
    }
}