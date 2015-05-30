using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ceenq.com.Assets.Services;
using ceenq.com.Core.Http;
using ceenq.com.Core.Utility;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.ManagementAPI.Controllers
{
    public class FilesApiController : ApiController
    {
        private readonly IAssetService _contentDocumentManager;
        public FilesApiController(IAssetService contentDocumentManager)
        {
            _contentDocumentManager = contentDocumentManager;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        [HttpPost]
        public HttpResponseMessage Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);

            IEnumerable<HttpContent> files = null;
            string basePath = null;
            Task.Factory
                .StartNew(() =>
                {
                    var result = Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider()).Result;

                    if (result.FormData != null && !String.IsNullOrWhiteSpace(result.FormData["basepath"]))
                        basePath = result.FormData["basepath"];

                    files = result.Files;
                },
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default)
                .Wait();

     
                foreach (var file in files)
                {
                    if (file.Headers.ContentDisposition.FileName == null) continue;
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var stream = file.ReadAsStreamAsync();

                    if (IsZipFile(filename))
                    {
                        var path = PathHelper.Combine(basePath, Path.GetDirectoryName(filename));
                        _contentDocumentManager.CreateFromZip(path, stream.Result, true);
                    }
                    else
                    {
                        _contentDocumentManager.CreateAsset(PathHelper.Combine(basePath, filename), stream.Result, true);
                    }
                }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage Download(string id)
        {
            try
            {
                var directoryItem = _contentDocumentManager.GetDirectoryItem(id);
                var result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(_contentDocumentManager.GetStream(id));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(directoryItem.Name)
                };

                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            try
            {
                var directoryItem = _contentDocumentManager.GetDirectoryItem(id);
                var result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(_contentDocumentManager.GetStream(id));
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(directoryItem.ContentType);

                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        private static bool IsZipFile(string filename)
        {
            var extension = Path.GetExtension(filename);
            return string.Equals(extension.TrimStart('.'), "zip", StringComparison.OrdinalIgnoreCase);
        }

    }

}
