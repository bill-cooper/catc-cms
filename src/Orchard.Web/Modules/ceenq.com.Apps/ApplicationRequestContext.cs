using System.Linq;
using ceenq.com.Apps.Models;
using ceenq.com.Apps.Services;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Tenants;
using Orchard;
using Orchard.Logging;

namespace ceenq.com.Apps
{
    public class ApplicationRequestContext : Component, IApplicationRequestContext
    {
        private readonly IApplicationManager _applicationManager;
        private readonly IApplicationService _applicationService;
        private readonly IAccountContext _accountContext;
        private readonly WorkContext _workContext;

        public ApplicationRequestContext(IOrchardServices orchardServices, IApplicationManager applicationManager, IApplicationService applicationService, IAccountContext accountContext)
        {
            _applicationManager = applicationManager;
            _applicationService = applicationService;
            _accountContext = accountContext;
            _workContext = orchardServices.WorkContext;
        }

        public bool IsApplicationRequest()
        {
            var url = _workContext.HttpContext.Request.RawUrl;
            return url.StartsWith(_accountContext.BaseCoreModulePath);
        }
        public bool IsAssetRequest()
        {
            var url = _workContext.HttpContext.Request.RawUrl;
            return url.StartsWith(_accountContext.AssetPath);
        }

        public bool IsCmsRequest()
        {
            var url = _workContext.HttpContext.Request.RawUrl;
            return url.StartsWith(_accountContext.CmsPath);
        }

        public string ApplicationRequestUrl()
        {
            var url = _workContext.HttpContext.Request.RawUrl.TrimStart('/');
            return _applicationService.ToPublicApplicationPath(Application, url);
        }

        private IApplication _application;
        public IApplication Application
        {
            get
            {
                if (_application == null)
                {

                    //TODO: wrap the following validation logic in some kind of application request validator framework
                    if (_workContext.HttpContext == null
                        || _workContext.HttpContext.Request == null
                        || _workContext.HttpContext.Request.Url == null)
                    {
                        var ex = new OrchardFatalException(T("Could not identify request url."));
                        Logger.Log(LogLevel.Fatal, ex, ex.Message);
                        throw ex;
                    }

                    var requestedApplicationName = _workContext.HttpContext.Request.Headers[Constants.ApplicationHeaderKey];
                    if (requestedApplicationName == null)
                    {
                        var ex = new OrchardFatalException(T("{0} was not provided with the request.", Constants.ApplicationHeaderKey));
                        Logger.Log(LogLevel.Fatal, ex, ex.Message);
                        throw ex;
                    }
                    requestedApplicationName = requestedApplicationName.ToLower();

                    _application =
                        _applicationManager.GetApplications(includeDynamicApplications:true)
                            .FirstOrDefault(a => a.Name == requestedApplicationName);

                    if (_application == null)
                    {
                        var ex = new OrchardFatalException(T("The {0} request header specifies application {1}, but this account does not have an application with that name.", Constants.AppHostHeaderKey, requestedApplicationName));
                        Logger.Log(LogLevel.Fatal, ex, ex.Message);
                        throw ex;
                    }
                }
                return _application;
            }
        }

    }
}