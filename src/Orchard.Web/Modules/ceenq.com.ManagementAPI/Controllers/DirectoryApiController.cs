using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ceenq.com.Assets.Models;
using ceenq.com.Assets.Services;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.ManagementAPI.Controllers
{
    //a/{account}/api/documents/{id}
    public class DirectoryApiController : ApiController
    {
        private readonly IAssetService _contentDocumentManager;

        public DirectoryApiController(IAssetService contentDocumentManager)
        {
            _contentDocumentManager = contentDocumentManager;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public HttpResponseMessage Get()
        {
            var models = _contentDocumentManager.GetDirectoryList();
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        public HttpResponseMessage Get(string id)
        {
            try
            {
                var model = _contentDocumentManager.GetDirectoryItem(id);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(DirectoryItemModel model)
        {
            return Save(model);
        }

        public HttpResponseMessage Post(DirectoryItemModel model)
        {
            return Save(model);
        }

        private HttpResponseMessage Save(DirectoryItemModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Id))
                {
                    var updatedModel = _contentDocumentManager.UpdateDirectoryItem(model);
                    return Request.CreateResponse(HttpStatusCode.OK, updatedModel);
                }
                else
                {
                    var updatedModel = _contentDocumentManager.CreateDirectoryItem(model);
                    return Request.CreateResponse(HttpStatusCode.Created, updatedModel);
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Compress(DirectoryItemModel model)
        {
            _contentDocumentManager.CompressDirectoryItem(model);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public HttpResponseMessage Delete(string id)
        {
            try
            {
                _contentDocumentManager.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


    }
}
