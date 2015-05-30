using System;
using System.Linq;
using ceenq.com.Assets.Services;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Models;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
using Orchard.FileSystems.Media;
using Orchard;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace ceenq.com.AssetImport.Services
{
    public interface IContentPurgeService : IDependency
    {
        void PurgeContent(string subdirectory);
    }
    public class ContentPurgeService : IContentPurgeService
    {
        private readonly IAssetService _contentDocumentManager;
        private readonly IAccountContext _accountContext;
        private readonly IOrchardServices _orchardServices;
        private readonly ITransactionManager _transactionManager;
        private readonly IContentManager _contentManager;
        private readonly IAssetManager _assetManager;
        public ContentPurgeService(IAssetService contentDocumentManager, IAccountContext accountContext, IOrchardServices orchardServices, ITransactionManager transactionManager, IContentManager contentManager, IAssetManager assetManager)
        {
            _contentDocumentManager = contentDocumentManager;
            _accountContext = accountContext;
            _orchardServices = orchardServices;
            _transactionManager = transactionManager;
            _contentManager = contentManager;
            _assetManager = assetManager;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public void PurgeContent(string subdirectory)
        {
            var contentDocuments = _contentDocumentManager.GetAssets().ToList();
            var contentDocumentList = contentDocuments.Where(contentDocument => contentDocument.Path.StartsWith(subdirectory.ToLower())).ToList();
            _orchardServices.Notifier.Information(T("Found {0} content documents to purge", contentDocumentList.Count));
            var successCount = 0;
            var failCount = 0;
            foreach (var contentDocument in contentDocumentList)
            {
                var contentDocSuccess = false;
                var fileSuccess = false;
                try
                {
                    var content = _contentManager.Query<TitlePart,TitlePartRecord>()
                        .Where(doc => doc.Title == contentDocument.Path)
                        .ForType("ContentDocument")
                        .Slice(0, 1)
                        .FirstOrDefault();
                    if (content == null)
                        throw new OrchardException(T("Could not find expected content document with path : {0}", contentDocument.Path));
                    _contentManager.Remove(content.ContentItem);
                    _orchardServices.ContentManager.Clear();
                    _transactionManager.RequireNew();
                    Logger.Information("Successfully purged {0} - {1} of {2}", contentDocument.Path, contentDocuments.IndexOf(contentDocument) + 1, contentDocuments.Count());
                    contentDocSuccess = true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to purge contentDocument with path: {0}", contentDocument.Path);
                    //Ensure a failed batch is rolled back
                    _transactionManager.Cancel();
                }
                try
                {
                    _assetManager.DeleteFile(contentDocument.Path);
                    fileSuccess = true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to purge share file with path: {0}", contentDocument.Path);
                }
                if (contentDocSuccess && fileSuccess) successCount ++;
                else failCount ++;
            }
            if (successCount > 0)
            {
                _orchardServices.Notifier.Information(T("Successfully purged {0} content documents and associated files", successCount));
            }
            if (failCount > 0)
            {
                _orchardServices.Notifier.Error(T("An error occurred during the purge for {0} items.  See error log for additional details.", failCount));
            }
            try
            {
                _assetManager.DeleteFolderContents(subdirectory);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to delete the contents of folder : {0} during the purge process.", subdirectory);
                _orchardServices.Notifier.Error(T("Failed to delete the contents of folder : {0} during the purge process.", subdirectory));
            }
        }
    }
}