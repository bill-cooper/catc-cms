using System.Web;
using ceenq.com.AzureCloudStorage.Models;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Environment;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc;

namespace ceenq.com.AzureCloudStorage.Services
{
    public class AzureStorageCredentialsProvider : IAssetStorageCredentialsProvider
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        public AzureStorageCredentialsProvider(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public string Username
        {
            get
            {
                if (_workContextAccessor.GetContext() == null)
                {
                    var context = ThreadSafeHttpContextAccessor.GetContext();
                    var workContextScope = _workContextAccessor.CreateWorkContextScope(new HttpContextWrapper(context));
                    return workContextScope.WorkContext.CurrentSite.As<CloudStorageSettingsPart>().StorageAccount;
                }
                return _workContextAccessor.GetContext().CurrentSite.As<CloudStorageSettingsPart>().StorageAccount;
            }
        }

        public string Password
        {
            get
            {
                if (_workContextAccessor.GetContext() == null)
                {
                    var context = ThreadSafeHttpContextAccessor.GetContext();
                    var workContextScope = _workContextAccessor.CreateWorkContextScope(new HttpContextWrapper(context));
                    return workContextScope.WorkContext.CurrentSite.As<CloudStorageSettingsPart>().StorageKey;
                }
                return _workContextAccessor.GetContext().CurrentSite.As<CloudStorageSettingsPart>().StorageKey;
            }
        }
    }
}