using ceenq.com.Core.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Settings.Models;
using Orchard.Environment.Configuration;
using Orchard.Settings;

namespace ceenq.com.Core.Accounts
{
    public interface IAccountContext : IDependency
    {
        string Account { get; }
        string AccountDomain { get; }
        string AbsoluteAccountBaseUrl { get; }
        string InternalAbsoluteBasePath { get; }
        string InternalAbsoluteApiPath { get; }
        string InternalAbsoluteCmsPath { get; }
        string InternalAbsoluteCmsAdminPath { get; }
        string BaseCoreModulePath { get; }
        string BaseCoreSystemDirectory { get; }
        string BaseCoreModuleDirectory { get; }
        string BaseServerDirectory { get; }
        string ServerAbsoluteBasePath { get; }
        string ServerAbsoluteAssetPath { get; }
        string ServerAbsoluteCoreSystemBasePath { get; }
        string ServerAbsoluteCoreSystemAssetPath { get; }
        string CmsToken { get; }
        string CmsAdminToken { get; }
        string AssetPath { get; }
        string ApiPath { get; }
        string CmsPath { get; }
        string CmsAdminPath { get; }
    }

    public class DefaultAccountContext : IAccountContext
    {
        private readonly ShellSettings _shellSettings;
        private readonly ISiteService _siteService;
        private readonly IWorkContextAccessor _workContextAccessor;
        public DefaultAccountContext(ShellSettings shellSettings, ISiteService siteService, IWorkContextAccessor workContextAccessor)
        {
            _shellSettings = shellSettings;
            _siteService = siteService;
            _workContextAccessor = workContextAccessor;
        }

        public string Account
        {
            get { return _shellSettings.Name; }
        }
        public string ServerUser
        {
            //REFACTOR: Do not hard code this
            get { return "azureuser"; }
        }

        public string AbsoluteAccountBaseUrl
        {
            get { return _siteService.GetSiteSettings().As<SiteSettingsPart>().BaseUrl; }
        }

        public string AccountDomain
        {
            get { return _workContextAccessor.GetContext().CurrentSite.As<CoreSettingsPart>().AccountDomain; }
        }

        public string AssetPath
        {
            get { return string.Format("/{0}/asset/", BaseCoreModuleDirectory); }
        }

        public string ApiPath
        {
            get { return string.Format("/{0}/api/", BaseCoreModuleDirectory); }
        }

        public string CmsPath
        {
            get { return string.Format("/{0}/cms/", BaseCoreModuleDirectory); }
        }

        public string CmsAdminPath
        {
            get { return "/admin/"; }
        }

        public string BaseCoreModulePath
        {
            get { return string.Format("/{0}/", BaseCoreModuleDirectory); }
        }
        public string BaseCoreModuleDirectory
        {
            get { return "cnq"; }
        }
        public string BaseCoreSystemDirectory
        {
            get { return "cnq"; }
        }
        public string BaseServerDirectory
        {
            get { return "home"; }
        }

        public string CmsToken
        {
            get { return "$cms"; }
        }
        public string CmsAdminToken
        {
            get { return "$cmsadmin"; }
        }

        public string ServerAbsoluteBasePath
        {
            get { return string.Format("/{0}/{1}/{2}", BaseServerDirectory, ServerUser, Account); }
        }

        public string ServerAbsoluteAssetPath
        {
            get { return string.Format("{0}{1}", ServerAbsoluteBasePath, AssetPath); }
        }

        public string ServerAbsoluteCoreSystemBasePath
        {
            get { return string.Format("/{0}/{1}/{2}", BaseServerDirectory, ServerUser, BaseCoreSystemDirectory); }
        }

        public string ServerAbsoluteCoreSystemAssetPath
        {
            get { return string.Format("{0}{1}", ServerAbsoluteCoreSystemBasePath, AssetPath); }
        }
        public string InternalAbsoluteBasePath
        {
            get { return string.Format("{0}{1}", AbsoluteAccountBaseUrl, BaseCoreModulePath); }
        }

        public string InternalAbsoluteApiPath
        {
            get { return string.Format("{0}{1}", AbsoluteAccountBaseUrl, ApiPath); }
        }

        public string InternalAbsoluteCmsPath
        {
            get { return string.Format("{0}{1}", AbsoluteAccountBaseUrl, CmsPath); }
        }

        public string InternalAbsoluteCmsAdminPath
        {
            get { return string.Format("{0}{1}", AbsoluteAccountBaseUrl, CmsAdminPath); }
        }

    }
}
