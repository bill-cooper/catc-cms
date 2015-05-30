using System.Linq;
using System.Net;
using System.Web.Mvc;
using Orchard;
using Orchard.MediaLibrary.Models;
using Orchard.Themes;

namespace ceenq.com.Media.Controllers
{
    [Themed]
    public class DownloadController : Controller
    {
        private readonly IOrchardServices _orchardServices;

        public DownloadController(
            IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        [HttpGet]
        public ActionResult Download(string mediaType, int mediaId, string fileName)
        {
            var media = _orchardServices.ContentManager.Query().ForPart<MediaPart>().Where<MediaPartRecord>(m => m.Id == mediaId && m.FileName == fileName).List().FirstOrDefault();
            if (media == null)
                return HttpNotFound();

            //currently, DownloadController is only handling download for audio media.
            // it is possible that other media types may need to be handled in the future
            // and may need special logic or different response content types.  I have provided
            // the parameter mediaType for this possible future need.  It is not used currently.

            if (Url.IsLocalUrl(media.MediaUrl))
                return File(media.MediaUrl, "application/octet-stream", media.FileName);
            else
            {
                var request = (HttpWebRequest)WebRequest.Create(media.MediaUrl);

                var response = (HttpWebResponse)request.GetResponse();
                if (request.ContentLength > 0)
                    response.ContentLength = request.ContentLength;

                var stream = response.GetResponseStream();
                return File(stream, "application/octet-stream", media.FileName);
            }
        }
    }
}