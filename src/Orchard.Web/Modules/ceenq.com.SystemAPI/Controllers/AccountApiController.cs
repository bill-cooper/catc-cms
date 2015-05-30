using System.Net;
using System.Net.Http;
using System.Web.Http;
using ceenq.com.SystemAPI.Models;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.SystemAPI.Controllers
{
    //TODO: This controller allows for public account creation.
    // This controller is disabled for now, but should be opened up
    // eventually
    public class AccountApiController : ApiController
    {
        public AccountApiController()
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public Localizer T { get; set; }

        [HttpPost]
        public HttpResponseMessage Verify(AccountVerification model)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, T("Account Verification Failed").Text);
        }

        [HttpPost]
        public HttpResponseMessage Create(AccountCreate accountCreate)
        {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, T("A failure occurred while creating your account.").Text);
        }
    }
}