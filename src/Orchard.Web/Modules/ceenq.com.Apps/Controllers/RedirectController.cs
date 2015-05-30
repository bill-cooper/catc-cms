using System.Web.Mvc;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.Apps.Controllers
{
    [ValidateInput(false)]
    public class RedirectController : Controller
    {

        public RedirectController()
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Index()
        {

            return RedirectToAction("Index", "Admin");
        }


    }
}
