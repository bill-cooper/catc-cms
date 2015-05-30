using System;
using System.Net;
using System.Web.Mvc;
using ceenq.com.Core.Accounts;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Notify;
using ceenq.com.AssetImport.Services;
using ceenq.com.AssetImport.ViewModels;
using System.Text;

namespace ceenq.com.AssetImport.Controllers
{
    public class AdminController : Controller
    {

        private readonly IContentImportService _contentImportService;
        private readonly IContentPurgeService _contentPurgeService;
        private readonly IAccountContext _accountContext;
        public AdminController(IOrchardServices orchardServices, IContentImportService contentImportService, IContentPurgeService contentPurgeService, IAccountContext accountContext)
        {
            Services = orchardServices;
            _contentImportService = contentImportService;
            _contentPurgeService = contentPurgeService;
            _accountContext = accountContext;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Import()
        {
            var model = new ImportViewModel {  Subdirectory = "/" };
            return View(model);
        }


        [HttpPost, ActionName("Import")]
        public ActionResult ImportPOST()
        {
            var viewModel = new ImportViewModel();

            UpdateModel(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            if (String.IsNullOrWhiteSpace(Request.Files[0].FileName) && String.IsNullOrWhiteSpace(viewModel.ImportUrl))
            {
                ModelState.AddModelError("File", T("Select a file or url to import from").ToString());
            }
            else if (!String.IsNullOrWhiteSpace(viewModel.ImportUrl))
            {

                try
                {
                    if (!viewModel.ImportUrl.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)) throw new Exception(T("The file must be a .zip file").Text);
                    var webClient = new WebClient { Encoding = Encoding.UTF8 };
                    var source = webClient.DownloadData(viewModel.ImportUrl);
                    _contentImportService.ImportContent(viewModel.Subdirectory, viewModel.OverwriteExisting, source);
                }
                catch (Exception e)
                {
                    Services.Notifier.Error(T("Uploading media file failed: {0}", e.Message));
                    return View(viewModel);
                }



            }
            else
            {
                foreach (string fileName in Request.Files)
                {
                    try
                    {
                        if (!Request.Files[fileName].FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)) throw new Exception(T("The file must be a .zip file").Text);
                        _contentImportService.ImportContent(viewModel.Subdirectory, viewModel.OverwriteExisting, Request.Files[fileName]);
                    }
                    catch (Exception e)
                    {
                        Services.Notifier.Error(T("Uploading media file failed: {0}", e.Message));
                        return View(viewModel);
                    }
                }
            }

            Services.Notifier.Information(T("Media file(s) uploaded"));
            return View(viewModel);
        }

        public ActionResult Purge()
        {
            var model = new PurgeViewModel { Subdirectory = _accountContext.AssetPath};

            return View(model);
        }


        [HttpPost, ActionName("Purge")]
        public ActionResult PurgePOST()
        {
            var viewModel = new PurgeViewModel();

            UpdateModel(viewModel);

   
            if (!ModelState.IsValid)
                return View(viewModel);

            _contentPurgeService.PurgeContent(viewModel.Subdirectory);

            Services.Notifier.Information(T("Files have been purged"));
            return View(viewModel);
        }
    }
}