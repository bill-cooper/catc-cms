using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ceenq.com.Assets.Models;
using ceenq.com.Assets.Services;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.ManagementAPI.Controllers
{
    public class AssetManagementApiController : ApiController
    {
        private readonly IAssetService _assetService;
        public AssetManagementApiController(IAssetService assetService)
        {
            _assetService = assetService;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public HttpResponseMessage Get()
        {
            var models = _assetService.GetAssets();
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        public HttpResponseMessage Get(string id)
        {
            try
            {
                var model = _assetService.Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(AssetModel model) {
            return Save(model);
        }

        public HttpResponseMessage Post(AssetModel model) {
            return Save(model);
        }

        private HttpResponseMessage Save(AssetModel model)
        {
            if(!Path.HasExtension(model.Path))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, T("Please provide a file name with an extension").Text);

            try
            {
                if (!string.IsNullOrEmpty(model.Id))
                {
                    _assetService.UpdateAsset(model);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    _assetService.CreateAsset(model);
                    return Request.CreateResponse(HttpStatusCode.Created, model);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(string id)
        {
            try
            {
                _assetService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
